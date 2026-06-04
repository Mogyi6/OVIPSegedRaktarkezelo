using Microsoft.Extensions.Options;
using Models.SOAPClient;
using System.Net.Http.Headers;
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

            debug.AppendLine("========== OVIP SOAP DEBUG START ==========");
            debug.AppendLine($"BaseUrl: {_options.BaseUrl}");
            debug.AppendLine($"Request: {request}");
            debug.AppendLine($"UserId: {_options.UserId}");
            debug.AppendLine($"WebshopId: {_options.WebshopId}");
            debug.AppendLine($"CallerIp: {_options.CallerIp}");
            debug.AppendLine($"ExtraData: {extraData}");
            debug.AppendLine($"LimitFrom: {limitFrom}");
            debug.AppendLine($"LimitTo: {limitTo}");

            var signatureResult = CreateSignatureWithDebug(request);
            var signature = signatureResult.Signature;

            debug.AppendLine("---------- SIGNATURE ----------");
            debug.AppendLine($"SignatureRaw: {signatureResult.Raw}");
            debug.AppendLine($"SignatureSha256: {signature}");

            var variants = new List<(string Name, string Xml, string? SoapAction, string? Accept)>
            {
                (
                    "Variant 1 - no namespace on input fields, no SOAPAction",
                    BuildSoapXmlSimple(request, signature, extraData, limitFrom, limitTo),
                    null,
                    null
                ),
                (
                    "Variant 2 - namespace on input fields, no SOAPAction",
                    BuildSoapXmlNamespacedInput(request, signature, extraData, limitFrom, limitTo),
                    null,
                    null
                ),
                (
                    "Variant 3 - PHP SOAP array style, no SOAPAction",
                    BuildSoapXmlPhpArrayStyle(request, signature, extraData, limitFrom, limitTo),
                    null,
                    null
                ),
                (
                    "Variant 4 - simple XML with SOAPAction",
                    BuildSoapXmlSimple(request, signature, extraData, limitFrom, limitTo),
                    "\"getRequest\"",
                    "*/*"
                ),
                (
                    "Variant 5 - param0 SOAP-ENC Struct with SOAPAction",
                    BuildSoapXmlParam0(request, signature, extraData, limitFrom, limitTo),
                    _options.BaseUrl?.TrimEnd('/') + "#getRequest",
                    "text/xml, application/xml, */*"
                )
            };

            foreach (var variant in variants)
            {
                debug.AppendLine();
                debug.AppendLine($"========== TRYING {variant.Name} ==========");
                debug.AppendLine("Request XML:");
                debug.AppendLine(variant.Xml);

                try
                {
                    using var content = new StringContent(variant.Xml, Encoding.UTF8, "text/xml");

                    using var requestMessage = new HttpRequestMessage(HttpMethod.Post, _options.BaseUrl)
                    {
                        Content = content
                    };

                    requestMessage.Headers.Accept.Clear();
                    if (!string.IsNullOrWhiteSpace(variant.Accept))
                    {
                        requestMessage.Headers.TryAddWithoutValidation("Accept", variant.Accept);
                    }
                    else
                    {
                        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                    }

                    if (!string.IsNullOrWhiteSpace(variant.SoapAction))
                    {
                        var soapActionValue = variant.SoapAction!.StartsWith("\"") && variant.SoapAction.EndsWith("\"")
                            ? variant.SoapAction
                            : "\"" + variant.SoapAction + "\"";

                        requestMessage.Headers.TryAddWithoutValidation("SOAPAction", soapActionValue);
                    }

                    debug.AppendLine("Request Headers:");
                    foreach (var header in requestMessage.Headers)
                    {
                        debug.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }

                    debug.AppendLine("Content Headers:");
                    foreach (var header in content.Headers)
                    {
                        debug.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }

                    var response = await _httpClient.SendAsync(requestMessage);
                    var xml = await response.Content.ReadAsStringAsync();

                    debug.AppendLine("Response:");
                    debug.AppendLine($"StatusCode: {(int)response.StatusCode} {response.StatusCode}");
                    debug.AppendLine($"IsSuccessStatusCode: {response.IsSuccessStatusCode}");
                    debug.AppendLine($"ContentLength: {response.Content.Headers.ContentLength}");
                    debug.AppendLine($"ContentType: {response.Content.Headers.ContentType}");

                    debug.AppendLine("Response Headers:");
                    foreach (var header in response.Headers)
                    {
                        debug.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }

                    debug.AppendLine("Response Content Headers:");
                    foreach (var header in response.Content.Headers)
                    {
                        debug.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }

                    debug.AppendLine("Response Body:");
                    debug.AppendLine(string.IsNullOrWhiteSpace(xml) ? "[EMPTY BODY]" : xml);

                    if (!response.IsSuccessStatusCode)
                    {
                        debug.AppendLine($"Result: HTTP error in {variant.Name}, trying next variant...");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(xml))
                    {
                        debug.AppendLine($"Result: Empty response in {variant.Name}, trying next variant...");
                        continue;
                    }

                    var extracted = ExtractReturnValue(xml);

                    debug.AppendLine("Extracted return value:");
                    debug.AppendLine(extracted);
                    debug.AppendLine("========== OVIP SOAP DEBUG END ==========");

                    return extracted;
                }
                catch (Exception ex)
                {
                    debug.AppendLine($"Exception in {variant.Name}:");
                    debug.AppendLine(ex.ToString());
                    debug.AppendLine("Trying next variant...");
                }
            }

            debug.AppendLine();
            debug.AppendLine("========== FINAL RESULT ==========");
            debug.AppendLine("All SOAP variants failed or returned empty response.");
            debug.AppendLine("========== OVIP SOAP DEBUG END ==========");

            throw new InvalidOperationException(debug.ToString());
        }

        private string BuildSoapXmlSimple(
            string request,
            string signature,
            object? extraData,
            int? limitFrom,
            int? limitTo)
        {
            var nsEnv = "http://schemas.xmlsoap.org/soap/envelope/";
            var ns1 = "https://www.ovip.hu/webshopAPI";

            var input = new XElement("input",
                new XElement("request", request),
                new XElement("user_id", _options.UserId),
                new XElement("webshop_id", _options.WebshopId),
                new XElement("signature", signature),
                new XElement("ip_cim", _options.CallerIp)
            );

            AddOptionalFields(input, extraData, limitFrom, limitTo);

            var soapDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement(XName.Get("Envelope", nsEnv),
                    new XAttribute(XNamespace.Xmlns + "SOAP-ENV", nsEnv),
                    new XAttribute(XNamespace.Xmlns + "ns1", ns1),
                    new XElement(XName.Get("Body", nsEnv),
                        new XElement(XName.Get("getRequest", ns1), input)
                    )
                )
            );

            return soapDoc.ToString(SaveOptions.DisableFormatting);
        }

        private string BuildSoapXmlNamespacedInput(
            string request,
            string signature,
            object? extraData,
            int? limitFrom,
            int? limitTo)
        {
            var nsEnv = "http://schemas.xmlsoap.org/soap/envelope/";
            var ns1 = "https://www.ovip.hu/webshopAPI";

            var input = new XElement(XName.Get("input", ns1),
                new XElement(XName.Get("request", ns1), request),
                new XElement(XName.Get("user_id", ns1), _options.UserId),
                new XElement(XName.Get("webshop_id", ns1), _options.WebshopId),
                new XElement(XName.Get("signature", ns1), signature),
                new XElement(XName.Get("ip_cim", ns1), _options.CallerIp)
            );

            AddOptionalFields(input, extraData, limitFrom, limitTo, ns1);

            var soapDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement(XName.Get("Envelope", nsEnv),
                    new XAttribute(XNamespace.Xmlns + "SOAP-ENV", nsEnv),
                    new XAttribute(XNamespace.Xmlns + "ns1", ns1),
                    new XElement(XName.Get("Body", nsEnv),
                        new XElement(XName.Get("getRequest", ns1), input)
                    )
                )
            );

            return soapDoc.ToString(SaveOptions.DisableFormatting);
        }

        private string BuildSoapXmlPhpArrayStyle(
            string request,
            string signature,
            object? extraData,
            int? limitFrom,
            int? limitTo)
        {
            var nsEnv = "http://schemas.xmlsoap.org/soap/envelope/";
            var ns1 = "https://www.ovip.hu/webshopAPI";
            var soapEnc = "http://schemas.xmlsoap.org/soap/encoding/";
            var xsi = "http://www.w3.org/2001/XMLSchema-instance";
            var xsd = "http://www.w3.org/2001/XMLSchema";

            var input = new XElement("input",
                new XAttribute(XName.Get("type", xsi), "SOAP-ENC:Array"),
                new XAttribute(XName.Get("arrayType", soapEnc), "xsd:anyType[5]"),
                PhpItem("request", request),
                PhpItem("user_id", _options.UserId),
                PhpItem("webshop_id", _options.WebshopId),
                PhpItem("signature", signature),
                PhpItem("ip_cim", _options.CallerIp)
            );

            var optionalCount = 0;

            if (extraData != null)
            {
                input.Add(PhpItem("extra_data", extraData.ToString() ?? ""));
                optionalCount++;
            }

            if (limitFrom.HasValue)
            {
                input.Add(PhpItem("limit_from", limitFrom.Value.ToString()));
                optionalCount++;
            }

            if (limitTo.HasValue)
            {
                input.Add(PhpItem("limit_to", limitTo.Value.ToString()));
                optionalCount++;
            }

            input.SetAttributeValue(XName.Get("arrayType", soapEnc), $"xsd:anyType[{5 + optionalCount}]");

            var soapDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement(XName.Get("Envelope", nsEnv),
                    new XAttribute(XNamespace.Xmlns + "SOAP-ENV", nsEnv),
                    new XAttribute(XNamespace.Xmlns + "ns1", ns1),
                    new XAttribute(XNamespace.Xmlns + "SOAP-ENC", soapEnc),
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                    new XAttribute(XNamespace.Xmlns + "xsd", xsd),
                    new XElement(XName.Get("Body", nsEnv),
                        new XElement(XName.Get("getRequest", ns1), input)
                    )
                )
            );

            return soapDoc.ToString(SaveOptions.DisableFormatting);
        }

        private string BuildSoapXmlParam0(
            string request,
            string signature,
            object? extraData,
            int? limitFrom,
            int? limitTo)
        {
            var nsEnv = "http://schemas.xmlsoap.org/soap/envelope/";
            var ns1 = _options.BaseUrl?.TrimEnd('/') ?? "https://www.ovip.hu/webshopAPI";
            var soapEnc = "http://schemas.xmlsoap.org/soap/encoding/";
            var xsi = "http://www.w3.org/2001/XMLSchema-instance";
            var xsd = "http://www.w3.org/2001/XMLSchema";

            var param0 = new XElement(XName.Get("param0", ns1),
                new XAttribute(XName.Get("type", xsi), "SOAP-ENC:Struct"),
                new XElement(XName.Get("request", ns1),
                    new XAttribute(XName.Get("type", xsi), "xsd:string"), request),
                new XElement(XName.Get("user_id", ns1),
                    new XAttribute(XName.Get("type", xsi), "xsd:string"), _options.UserId),
                new XElement(XName.Get("signature", ns1),
                    new XAttribute(XName.Get("type", xsi), "xsd:string"), signature),
                new XElement(XName.Get("webshop_id", ns1),
                    new XAttribute(XName.Get("type", xsi), "xsd:string"), _options.WebshopId)
            );

            if (extraData != null)
            {
                param0.Add(new XElement(XName.Get("extra_data", ns1),
                    new XAttribute(XName.Get("type", xsi), "xsd:string"), extraData.ToString()));
            }

            if (limitFrom.HasValue)
            {
                param0.Add(new XElement(XName.Get("limit_from", ns1),
                    new XAttribute(XName.Get("type", xsi), "xsd:string"), limitFrom.Value.ToString()));
            }

            if (limitTo.HasValue)
            {
                param0.Add(new XElement(XName.Get("limit_to", ns1),
                    new XAttribute(XName.Get("type", xsi), "xsd:string"), limitTo.Value.ToString()));
            }

            var soapDoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                new XElement(XName.Get("Envelope", nsEnv),
                    new XAttribute(XNamespace.Xmlns + "SOAP-ENV", nsEnv),
                    new XAttribute(XNamespace.Xmlns + "ns1", ns1),
                    new XAttribute(XNamespace.Xmlns + "SOAP-ENC", soapEnc),
                    new XAttribute(XNamespace.Xmlns + "xsi", xsi),
                    new XAttribute(XNamespace.Xmlns + "xsd", xsd),
                    new XAttribute(XName.Get("encodingStyle", nsEnv), soapEnc),
                    new XElement(XName.Get("Body", nsEnv),
                        new XElement(XName.Get("getRequest", ns1), param0)
                    )
                )
            );

            return soapDoc.ToString(SaveOptions.DisableFormatting);
        }

        private static XElement PhpItem(string key, string value)
        {
            var xsi = "http://www.w3.org/2001/XMLSchema-instance";

            return new XElement("item",
                new XElement("key", key),
                new XElement("value",
                    new XAttribute(XName.Get("type", xsi), "xsd:string"),
                    value
                )
            );
        }

        private void AddOptionalFields(
            XElement input,
            object? extraData,
            int? limitFrom,
            int? limitTo,
            string? ns = null)
        {
            XName Name(string name)
            {
                return ns == null ? name : XName.Get(name, ns);
            }

            if (extraData != null)
            {
                input.Add(new XElement(Name("extra_data"), extraData.ToString()));
            }

            if (limitFrom.HasValue)
            {
                input.Add(new XElement(Name("limit_from"), limitFrom.Value));
            }

            if (limitTo.HasValue)
            {
                input.Add(new XElement(Name("limit_to"), limitTo.Value));
            }
        }

        private (string Raw, string Signature) CreateSignatureWithDebug(string request)
        {
            var raw = $"{_options.UserId}{_options.WebshopId}{_options.AuthCode}{request}{_options.CallerIp}";
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
            var signature = Convert.ToHexString(hash).ToLower();

            return (raw, signature);
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