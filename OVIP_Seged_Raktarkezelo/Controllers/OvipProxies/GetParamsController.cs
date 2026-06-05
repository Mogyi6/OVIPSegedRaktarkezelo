using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OVIP_Seged_Raktarkezelo.Controllers.OvipProxies
{
    [ApiController]
    [Route("api/ovip/getParams")]
    public class GetParamsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] Logic.Logic.Sync.IOvipSyncLogic syncLogic)
        {
            var resultJson = await syncLogic.SyncParametersAsync();
            return Content(resultJson, "application/json");
        }
    }
}
