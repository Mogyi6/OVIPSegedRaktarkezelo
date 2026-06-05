using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace OVIP_Seged_Raktarkezelo.Controllers.OvipProxies
{
    [ApiController]
    [Route("api/ovip/getProducts")]
    public class GetProductsController : ControllerBase
    {
        private const string PhpProxyBase = "http://72.60.176.243:5000/";

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? extra_data, [FromQuery] int? limit_from, [FromQuery] int? limit_to)
        {
            var url = PhpProxyBase + "?request=getProducts";
            if (!string.IsNullOrEmpty(extra_data)) url += "&extra_data=" + System.Uri.EscapeDataString(extra_data);
            if (limit_from.HasValue) url += "&limit_from=" + limit_from.Value;
            if (limit_to.HasValue) url += "&limit_to=" + limit_to.Value;

            using var client = new HttpClient();
            var resp = await client.GetAsync(url);
            var body = await resp.Content.ReadAsStringAsync();
            return Content(body, "application/json");
        }
    }
}
