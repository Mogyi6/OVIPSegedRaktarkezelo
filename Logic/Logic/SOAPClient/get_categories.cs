using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security;

namespace Logic.Logic.SOAPClient
{
    public class OvipSoapExample
    {
    public static async Task Main()
    {
        var result = await GetCategoriesAsync();
        Console.WriteLine(result);
    }

    public static async Task<string> GetCategoriesAsync()
    {
        // --------------------
        // Credentials
        // --------------------
        string authCode = "555_EUXzyI6HGN6aqIVV";   // Auth Code
        string userId = "555";                     // User ID
        string webshopId = "596";                  // Webshop ID
        string ipAddress = "72.60.176.243";        // Server IP

        // --------------------
        // API url
        // --------------------
        string soapLink = "https://www.ovip.hu/webshopAPI/";

        // --------------------
        // Sent data
        // --------------------
        string request = "getCategories";

        string signatureBase = (userId + webshopId + authCode + request + ipAddress).Trim();
        string signature = Sha256Hex(signatureBase);

        string soapEnvelope = BuildSoapEnvelope(
            soapLink,
            request,
            userId,
            signature,
            webshopId
        );

        using var handler = new HttpClientHandler
        {
            // Fontos: ne kövesse vakon a redirectet, mert SOAP POST-nál gondot okozhat.
            AllowAutoRedirect = false
        };

        using var client = new HttpClient(handler);

        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, soapLink);

        httpRequest.Content = new StringContent(
            soapEnvelope,
            Encoding.UTF8,
            "text/xml"
        );

        // PHP SoapClient non-WSDL módban jellemzően ilyen SOAPAction-t küld:
        httpRequest.Headers.TryAddWithoutValidation(
            "SOAPAction",
            $"\"{soapLink}#getRequest\""
        );

        httpRequest.Headers.TryAddWithoutValidation("Accept", "text/xml, application/xml, */*");

        try
        {
            HttpResponseMessage response = await client.SendAsync(httpRequest);
            string responseBody = await response.Content.ReadAsStringAsync();

            return $"HTTP status: {(int)response.StatusCode} {response.StatusCode}\n\nResponse:\n{responseBody}";
        }
        catch (Exception ex)
        {
            return $"Hiba történt:\n{ex.Message}";
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