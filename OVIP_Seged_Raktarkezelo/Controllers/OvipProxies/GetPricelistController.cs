using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OVIP_Seged_Raktarkezelo.Controllers.OvipProxies
{
    [ApiController]
    [Route("api/ovip/getPricelist")]
    public class GetPricelistController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] Logic.Logic.Sync.IOvipSyncLogic syncLogic)
        {
            // Persist price list meta and prices
            var listsJson = await syncLogic.SyncPriceListsAsync();
            var pricesJson = await syncLogic.SyncPriceListPricesAsync();

            return Content(pricesJson, "application/json");
        }
    }
}
