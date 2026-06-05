using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OVIP_Seged_Raktarkezelo.Controllers.OvipProxies
{
    [ApiController]
    [Route("api/ovip/getProducts")]
    public class GetProductsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] Logic.Logic.Sync.IOvipSyncLogic syncLogic, [FromQuery] string? extra_data, [FromQuery] int? limit_from, [FromQuery] int? limit_to)
        {
            var resultJson = await syncLogic.SyncProductsAsync(extra_data, limit_from, limit_to);
            return Content(resultJson, "application/json");
        }
    }
}
