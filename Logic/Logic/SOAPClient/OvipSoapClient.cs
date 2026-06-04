using Microsoft.Extensions.Options;
using Models.SOAPClient;
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

            var signatureBase =
                $"{_options.UserId}{_options.WebshopId}{_options.AuthCode}{request}{_options.CallerIp}".Trim();

            var signature = Sha256Hex(signatureBase);

            var soapXml = BuildPhpArraySoapEnvelope(
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
            debug.AppendLine(soapXml);

            try
            {
                using var httpRequest = new HttpRequestMessage(HttpMethod.Post, soapLink);

                httpRequest.Content = new StringContent(
                    soapXml,
                    Encoding.UTF8,
                    "text/xml"
                );

                httpRequest.Headers.TryAddWithoutValidation(
                    "SOAPAction",
                    $"\"{soapLink}#getRequest\""
                );

                httpRequest.Headers.TryAddWithoutValidation(
                    "Accept",
                    "text/xml, application/xml, */*"
                );

                debug.AppendLine("---------- REQUEST HEADERS ----------");
                foreach (var header in httpRequest.Headers)
                {
                    debug.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                debug.AppendLine("---------- CONTENT HEADERS ----------");
                foreach (var header in httpRequest.Content.Headers)
                {
                    debug.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }

                var response = await _httpClient.SendAsync(httpRequest);
                var body = await response.Content.ReadAsStringAsync();

                debug.AppendLine("---------- RESPONSE ----------");
                debug.AppendLine($"StatusCode: {(int)response.StatusCode} {response.StatusCode}");
                debug.AppendLine($"IsSuccessStatusCode: {response.IsSuccessStatusCode}");
                debug.AppendLine($"ContentLength: {response.Content.Headers.ContentLength}");
                debug.AppendLine($"ContentType: {response.Content.Headers.ContentType}");

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
                debug.AppendLine(string.IsNullOrWhiteSpace(body) ? "[EMPTY BODY]" : body);

                if (!response.IsSuccessStatusCode)
                    throw new InvalidOperationException(debug.ToString());

                if (string.IsNullOrWhiteSpace(body))
                {
                    debug.AppendLine("OVIP üres választ adott.");
                    throw new InvalidOperationException(debug.ToString());
                }

                var extracted = ExtractReturnValue(body);

                debug.AppendLine("---------- EXTRACTED ----------");
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
                ? "https://www.ovip.innovip.hu/webshopAPI/"
                : baseUrl.Trim();

            if (!url.EndsWith("/"))
                url += "/";

            return url;
        }

        private static string BuildPhpArraySoapEnvelope(
            string soapLink,
            string request,
            string userId,
            string signature,
            string webshopId,
            object? extraData,
            int? limitFrom,
            int? limitTo)
        {
            var items = new List<(string Key, string Value)>
            {
                ("request", request),
                ("user_id", userId),
                ("signature", signature),
                ("webshop_id", webshopId)
            };

            if (extraData != null)
                items.Add(("extra_data", extraData.ToString() ?? ""));

            if (limitFrom.HasValue)
                items.Add(("limit_from", limitFrom.Value.ToString()));

            if (limitTo.HasValue)
                items.Add(("limit_to", limitTo.Value.ToString()));

            var itemXml = new StringBuilder();

            foreach (var item in items)
            {
                itemXml.AppendLine($@"
                <item>
                    <key xsi:type=""xsd:string"">{SecurityElement.Escape(item.Key)}</key>
                    <value xsi:type=""xsd:string"">{SecurityElement.Escape(item.Value)}</value>
                </item>");
            }

            return $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<SOAP-ENV:Envelope 
    xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
    xmlns:ns1=""{SecurityElement.Escape(soapLink)}""
    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
    xmlns:SOAP-ENC=""http://schemas.xmlsoap.org/soap/encoding/""
    SOAP-ENV:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/"">
    <SOAP-ENV:Body>
        <ns1:getRequest>
            <param0 xsi:type=""SOAP-ENC:Array"" SOAP-ENC:arrayType=""xsd:anyType[{items.Count}]"">
{itemXml}
            </param0>
        </ns1:getRequest>
    </SOAP-ENV:Body>
</SOAP-ENV:Envelope>";
        }

        private static string Sha256Hex(string input)
        {
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            var sb = new StringBuilder();

            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

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