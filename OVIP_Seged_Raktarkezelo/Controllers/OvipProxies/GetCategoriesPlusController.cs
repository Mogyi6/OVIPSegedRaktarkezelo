using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OVIP_Seged_Raktarkezelo.Controllers.OvipProxies
{
    [ApiController]
    [Route("api/ovip/getCategoriesPlus")]
    public class GetCategoriesPlusController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] Logic.Logic.Sync.IOvipSyncLogic syncLogic)
        {
            var resultJson = await syncLogic.SyncCategoryConnectionsAsync();
            return Content(resultJson, "application/json");
        }
    }
}
