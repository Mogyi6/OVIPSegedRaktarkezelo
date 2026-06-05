using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OVIP_Seged_Raktarkezelo.Controllers.OvipProxies
{
    [ApiController]
    [Route("api/ovip/getStock")]
    public class GetStockController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] Logic.Logic.Sync.IOvipSyncLogic syncLogic, [FromQuery] string? extra_data)
        {
            var resultJson = await syncLogic.CallPhpProxyAsync("getStock", extra_data);
            return Content(resultJson, "application/json");
        }
    }
}
