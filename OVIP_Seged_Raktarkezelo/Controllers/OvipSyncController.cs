using Logic.Logic.SOAPClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.SOAPClient;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/ovip-sync")]
    public class OvipSyncController : ControllerBase
    {
        private readonly IOvipSyncLogic _ovipSyncLogic;
        private readonly OvipOptions _ovipOptions;

        public OvipSyncController(
            IOvipSyncLogic ovipSyncLogic,
            IOptions<OvipOptions> ovipOptions)
        {
            _ovipSyncLogic = ovipSyncLogic;
            _ovipOptions = ovipOptions.Value;
        }

        [HttpPost("all")]
        public async Task<IActionResult> SyncAll()
        {
            try
            {
                await _ovipSyncLogic.SyncAllAsync();

                return Ok(new
                {
                    message = "OVIP teljes szinkron sikeresen lefutott."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "OVIP szinkron hiba történt.",
                    error = ex.Message,
                    exception = ex.GetType().Name
                });
            }
        }

        [HttpGet("categories/raw-custom")]
        public async Task<IActionResult> GetCategoriesRawCustom()
        {
            var soapLink = _ovipOptions.BaseUrl.TrimEnd('/') + "/";
            var request = "getCategories";
            var signatureBase = ($"{_ovipOptions.UserId}{_ovipOptions.WebshopId}{_ovipOptions.AuthCode}{request}{_ovipOptions.CallerIp}").Trim();
            var signature = Sha256Hex(signatureBase);

            var soapEnvelope = BuildSoapEnvelope(
                soapLink,
                request,
                _ovipOptions.UserId,
                signature,
                _ovipOptions.WebshopId
            );

            try
            {
                using var handler = new HttpClientHandler
                {
                    AllowAutoRedirect = false
                };
                using var client = new HttpClient(handler);
                using var httpRequest = new HttpRequestMessage(HttpMethod.Post, soapLink);

                httpRequest.Content = new StringContent(
                    soapEnvelope,
                    Encoding.UTF8,
                    "text/xml"
                );

                httpRequest.Headers.TryAddWithoutValidation(
                    "SOAPAction",
                    $"\"{soapLink}#getRequest\"");

                httpRequest.Headers.TryAddWithoutValidation(
                    "Accept",
                    "text/xml, application/xml, */*");

                var response = await client.SendAsync(httpRequest);
                var responseBody = await response.Content.ReadAsStringAsync();

                return Content(responseBody, response.Content.Headers.ContentType?.MediaType ?? "text/xml");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "OVIP getCategories raw custom hiba.",
                    error = ex.Message,
                    exception = ex.GetType().Name
                });
            }
        }

        private static string Sha256Hex(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hash = SHA256.HashData(bytes);

            StringBuilder sb = new StringBuilder();

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }

        private static string BuildSoapEnvelope(
            string soapLink,
            string request,
            string userId,
            string signature,
            string webshopId
        )
        {
            string xmlSoapLink = SecurityElement.Escape(soapLink);
            string xmlRequest = SecurityElement.Escape(request);
            string xmlUserId = SecurityElement.Escape(userId);
            string xmlSignature = SecurityElement.Escape(signature);
            string xmlWebshopId = SecurityElement.Escape(webshopId);

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
            <param0 xsi:type=""SOAP-ENC:Array"" SOAP-ENC:arrayType=""xsd:anyType[4]"">
                <item>
                    <key xsi:type=""xsd:string"">request</key>
                    <value xsi:type=""xsd:string"">{xmlRequest}</value>
                </item>
                <item>
                    <key xsi:type=""xsd:string"">user_id</key>
                    <value xsi:type=""xsd:string"">{xmlUserId}</value>
                </item>
                <item>
                    <key xsi:type=""xsd:string"">signature</key>
                    <value xsi:type=""xsd:string"">{xmlSignature}</value>
                </item>
                <item>
                    <key xsi:type=""xsd:string"">webshop_id</key>
                    <value xsi:type=""xsd:string"">{xmlWebshopId}</value>
                </item>
            </param0>
        </ns1:getRequest>
    </SOAP-ENV:Body>
</SOAP-ENV:Envelope>";
        }
    }
}