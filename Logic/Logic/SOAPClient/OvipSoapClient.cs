using Microsoft.Extensions.Options;
using Models.SOAPClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logic.Logic.SOAPClient
{
    public class OvipSoapClient : IOvipSoapClient
    {
        private readonly HttpClient _httpClient;
        private readonly OvipOptions _options;

        public OvipSoapClient(HttpClient httpClient, IOptions<OvipOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string> GetRequestAsync(
            string request,
            object? extraData = null,
            int? limitFrom = null,
            int? limitTo = null)
        {
            var escapedRequest = System.Security.SecurityElement.Escape(request);
            var escapedUserId = System.Security.SecurityElement.Escape(_options.UserId);
            var escapedWebshopId = System.Security.SecurityElement.Escape(_options.WebshopId);
            var escapedCallerIp = System.Security.SecurityElement.Escape(_options.CallerIp);
            var signature = CreateSignature(request);

            var extraXml = extraData == null
                ? ""
                : $"<extra_data>{System.Security.SecurityElement.Escape(extraData.ToString())}</extra_data>";

            var limitFromXml = limitFrom.HasValue
                ? $"<limit_from>{limitFrom.Value}</limit_from>"
                : "";

            var limitToXml = limitTo.HasValue
                ? $"<limit_to>{limitTo.Value}</limit_to>"
                : "";

            var soapXml = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<SOAP-ENV:Envelope 
    xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
    xmlns:ns1=""https://www.ovip.hu/webshopAPI"">
  <SOAP-ENV:Body>
    <ns1:getRequest>
      <input>
        <request>{escapedRequest}</request>
        <user_id>{escapedUserId}</user_id>
        <webshop_id>{escapedWebshopId}</webshop_id>
        <signature>{signature}</signature>
        <ip_cim>{escapedCallerIp}</ip_cim>
        {extraXml}
        {limitFromXml}
        {limitToXml}
      </input>
    </ns1:getRequest>
  </SOAP-ENV:Body>
</SOAP-ENV:Envelope>";

            using var content = new StringContent(soapXml, Encoding.UTF8, "text/xml");

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, _options.BaseUrl)
            {
                Content = content
            };

            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
            requestMessage.Headers.Add("SOAPAction", "\"getRequest\"");

            System.Diagnostics.Debug.WriteLine(
                $"OVIP SOAP Request - input: {request}, signature: {signature}, extra_data: {extraXml}, limit_from: {limitFrom}, limit_to: {limitTo}" + Environment.NewLine + soapXml);

            string xml;

            try
            {
                var response = await _httpClient.SendAsync(requestMessage);
                xml = await response.Content.ReadAsStringAsync();

                System.Diagnostics.Debug.WriteLine(
                    $"OVIP SOAP Response - Status: {response.StatusCode}, ContentLength: {response.Content.Headers.ContentLength}, Body: {xml}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(
                        $"OVIP SOAP call failed ({response.StatusCode}). Response body:\n{xml}");
                }

                if (string.IsNullOrWhiteSpace(xml))
                {
                    throw new InvalidOperationException(
                        $"OVIP SOAP response was empty (Status: {response.StatusCode}). The request body was: " + Environment.NewLine + soapXml);
                }
            }
            catch (Exception ex)
            {
                var errorMessage = $"OVIP SOAP request failed. Request XML:\n{soapXml}\nException: {ex}";
                System.Diagnostics.Debug.WriteLine(errorMessage);
                throw new InvalidOperationException(errorMessage, ex);
            }

            try
            {
                return ExtractReturnValue(xml);
            }
            catch (Exception ex)
            {
                var errorMessage = $"OVIP SOAP response parsing failed. Request XML:\n{soapXml}\nResponse XML:\n{xml}\nException: {ex}";
                System.Diagnostics.Debug.WriteLine(errorMessage);
                throw new InvalidOperationException(errorMessage, ex);
            }
        }

        private string CreateSignature(string request)
        {
            var raw = $"{_options.UserId}{_options.WebshopId}{_options.AuthCode}{request}{_options.CallerIp}";
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(raw));

            return Convert.ToHexString(hash).ToLower();
        }

        private static string ExtractReturnValue(string xml)
        {
            var doc = XDocument.Parse(xml);

            var returnNode = doc
                .Descendants()
                .FirstOrDefault(x =>
                    x.Name.LocalName.Equals("return", StringComparison.OrdinalIgnoreCase) ||
                    x.Name.LocalName.Equals("getRequestReturn", StringComparison.OrdinalIgnoreCase));

            return returnNode?.Value ?? xml;
        }
    }
}
