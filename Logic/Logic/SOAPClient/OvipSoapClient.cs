using Microsoft.Extensions.Options;
using Models.SOAPClient;
using System;
using System.Collections.Generic;
using System.Linq;
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

            var soapXml = $"""
        <?xml version="1.0" encoding="UTF-8"?>
        <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/">
            <soapenv:Body>
                <getRequest>
                    <input>
                        <request>{request}</request>
                        <user_id>{_options.UserId}</user_id>
                        <webshop_id>{_options.WebshopId}</webshop_id>
                        <signature>{signature}</signature>
                        <ip_cim>{_options.CallerIp}</ip_cim>
                        {extraXml}
                        {limitFromXml}
                        {limitToXml}
                    </input>
                </getRequest>
            </soapenv:Body>
        </soapenv:Envelope>
        """;

            using var content = new StringContent(soapXml, Encoding.UTF8, "text/xml");

            var response = await _httpClient.PostAsync(_options.BaseUrl, content);
            var xml = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            return ExtractReturnValue(xml);
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
