using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Security;

public class OvipSoapExample
{
    public static async Task Main()
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

            Console.WriteLine("HTTP status: " + (int)response.StatusCode + " " + response.StatusCode);

            if ((int)response.StatusCode >= 300 && (int)response.StatusCode < 400)
            {
                Console.WriteLine("Redirect történt!");
                Console.WriteLine("Location: " + response.Headers.Location);
            }

            Console.WriteLine("Response:");
            Console.WriteLine(responseBody);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Hiba történt:");
            Console.WriteLine(ex.Message);
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
            <param0 xsi:type=""SOAP-ENC:Struct"">
                <request xsi:type=""xsd:string"">{xmlRequest}</request>
                <user_id xsi:type=""xsd:string"">{xmlUserId}</user_id>
                <signature xsi:type=""xsd:string"">{xmlSignature}</signature>
                <webshop_id xsi:type=""xsd:string"">{xmlWebshopId}</webshop_id>
            </param0>
        </ns1:getRequest>
    </SOAP-ENV:Body>
</SOAP-ENV:Envelope>";
    }
}