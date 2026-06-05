using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace OVIP_Seged_Raktarkezelo.Controllers.OvipProxies
{
    [ApiController]
    [Route("api/ovip/getStock")]
    public class GetStockController : ControllerBase
    {
        private const string PhpProxyBase = "http://72.60.176.243:5000/";

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? extra_data)
        {
            var url = PhpProxyBase + "?request=getStock";
            if (!string.IsNullOrEmpty(extra_data)) url += "&extra_data=" + System.Uri.EscapeDataString(extra_data);
            using var client = new HttpClient();
            var resp = await client.GetAsync(url);
            var body = await resp.Content.ReadAsStringAsync();
            return Content(body, "application/json");
        }
    }
}
