using Microsoft.Extensions.Options;
using Models.SOAPClient;
using System.Net.Http.Headers;
using System.Security;
using System.Security.Cryptography;
using System.Text;
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
            var debug = new StringBuilder();

            var soapLink = NormalizeSoapLink(_options.BaseUrl);
            var signatureBase = $"{_options.UserId}{_options.WebshopId}{_options.AuthCode}{request}{_options.CallerIp}".Trim();
            var signature = Sha256Hex(signatureBase);

            var soapEnvelope = BuildOvipOfficialSoapEnvelope(
                soapLink,
                request,
                _options.UserId,
                signature,
                _options.WebshopId,
                extraData,
                limitFrom,
                limitTo
            );

            debug.AppendLine("========== OVIP SOAP DEBUG START ==========");
            debug.AppendLine($"SOAP Link: {soapLink}");
            debug.AppendLine($"Request: {request}");
            debug.AppendLine($"UserId: {_options.UserId}");
            debug.AppendLine($"WebshopId: {_options.WebshopId}");
            debug.AppendLine($"CallerIp: {_options.CallerIp}");
            debug.AppendLine($"ExtraData: {extraData}");
            debug.AppendLine($"LimitFrom: {limitFrom}");
            debug.AppendLine($"LimitTo: {limitTo}");
            debug.AppendLine("---------- SIGNATURE ----------");
            debug.AppendLine($"SignatureBase: {signatureBase}");
            debug.AppendLine($"SignatureSha256: {signature}");
            debug.AppendLine("---------- REQUEST XML ----------");
            debug.AppendLine(soapEnvelope);

            try
            {
                using var requestMessage = new HttpRequestMessage(HttpMethod.Post, soapLink);

                requestMessage.Content = new StringContent(
                    soapEnvelope,
                    Encoding.UTF8,
                    "text/xml"
                );

                requestMessage.Headers.TryAddWithoutValidation(
                    "SOAPAction",
                    $"\"{soapLink}#getRequest\""
                );

                requestMessage.Headers.TryAddWithoutValidation(
                    "Accept",
                    "text/xml, application/xml, */*"
                );

                debug.AppendLine("---------- REQUEST HEADERS ----------");

                foreach (var header in requestMessage.Headers)
                {
                    debug.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                debug.AppendLine("---------- CONTENT HEADERS ----------");

                foreach (var header in requestMessage.Content.Headers)
                {
                    debug.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                var response = await _httpClient.SendAsync(requestMessage);
                var responseBody = await response.Content.ReadAsStringAsync();

                debug.AppendLine("---------- RESPONSE ----------");
                debug.AppendLine($"StatusCode: {(int)response.StatusCode} {response.StatusCode}");
                debug.AppendLine($"IsSuccessStatusCode: {response.IsSuccessStatusCode}");
                debug.AppendLine($"ContentLength: {response.Content.Headers.ContentLength}");
                debug.AppendLine($"ContentType: {response.Content.Headers.ContentType}");

                if ((int)response.StatusCode >= 300 && (int)response.StatusCode < 400)
                {
                    debug.AppendLine("Redirect történt!");
                    debug.AppendLine($"Location: {response.Headers.Location}");
                }

                debug.AppendLine("---------- RESPONSE HEADERS ----------");

                foreach (var header in response.Headers)
                {
                    debug.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                debug.AppendLine("---------- RESPONSE CONTENT HEADERS ----------");

                foreach (var header in response.Content.Headers)
                {
                    debug.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                debug.AppendLine("---------- RESPONSE BODY ----------");
                debug.AppendLine(string.IsNullOrWhiteSpace(responseBody) ? "[EMPTY BODY]" : responseBody);

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException(debug.ToString());
                }

                if (string.IsNullOrWhiteSpace(responseBody))
                {
                    debug.AppendLine("---------- RESULT ----------");
                    debug.AppendLine("Az OVIP HTTP 200 OK választ adott, de a body üres.");
                    debug.AppendLine("Ez általában OVIP oldali aktiválási/IP/AuthCode/Webshop beállítási gond.");
                    throw new InvalidOperationException(debug.ToString());
                }

                var extracted = ExtractReturnValue(responseBody);

                debug.AppendLine("---------- EXTRACTED RETURN VALUE ----------");
                debug.AppendLine(extracted);
                debug.AppendLine("========== OVIP SOAP DEBUG END ==========");

                return extracted;
            }
            catch (Exception ex)
            {
                debug.AppendLine("---------- EXCEPTION ----------");
                debug.AppendLine(ex.ToString());
                debug.AppendLine("========== OVIP SOAP DEBUG END ==========");

                throw new InvalidOperationException(debug.ToString(), ex);
            }
        }

        private static string NormalizeSoapLink(string? baseUrl)
        {
            var url = string.IsNullOrWhiteSpace(baseUrl)
                ? "https://www.ovip.hu/webshopAPI/"
                : baseUrl.Trim();

            if (!url.EndsWith("/"))
            {
                url += "/";
            }

            return url;
        }

        private static string BuildOvipOfficialSoapEnvelope(
            string soapLink,
            string request,
            string userId,
            string signature,
            string webshopId,
            object? extraData,
            int? limitFrom,
            int? limitTo)
        {
            var xmlSoapLink = SecurityElement.Escape(soapLink);
            var xmlRequest = SecurityElement.Escape(request);
            var xmlUserId = SecurityElement.Escape(userId);
            var xmlSignature = SecurityElement.Escape(signature);
            var xmlWebshopId = SecurityElement.Escape(webshopId);

            var optionalXml = new StringBuilder();

            if (extraData != null)
            {
                optionalXml.AppendLine($@"                <extra_data xsi:type=""xsd:string"">{SecurityElement.Escape(extraData.ToString())}</extra_data>");
            }

            if (limitFrom.HasValue)
            {
                optionalXml.AppendLine($@"                <limit_from xsi:type=""xsd:string"">{limitFrom.Value}</limit_from>");
            }

            if (limitTo.HasValue)
            {
                optionalXml.AppendLine($@"                <limit_to xsi:type=""xsd:string"">{limitTo.Value}</limit_to>");
            }

            return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<SOAP-ENV:Envelope 
    xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
    xmlns:ns1=""{xmlSoapLink}""
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
    SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
    <SOAP-ENV:Body>
        <ns1:getRequest>
            <param0 xsi:type=""SOAP-ENC:Struct"">
                <request xsi:type=""xsd:string"">{xmlRequest}</request>
                <user_id xsi:type=""xsd:string"">{xmlUserId}</user_id>
                <signature xsi:type=""xsd:string"">{xmlSignature}</signature>
                <webshop_id xsi:type=""xsd:string"">{xmlWebshopId}</webshop_id>
{optionalXml}            </param0>
        </ns1:getRequest>
    </SOAP-ENV:Body>
</SOAP-ENV:Envelope>";
        }

        private static string Sha256Hex(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = SHA256.HashData(bytes);

            var sb = new StringBuilder();

            foreach (var b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
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