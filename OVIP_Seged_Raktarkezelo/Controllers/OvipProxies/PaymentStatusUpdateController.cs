using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace OVIP_Seged_Raktarkezelo.Controllers.OvipProxies
{
    [ApiController]
    [Route("api/ovip/paymentStatusUpdate")]
    public class PaymentStatusUpdateController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get([FromServices] Logic.Logic.Sync.IOvipSyncLogic syncLogic, [FromQuery] string? extra_data)
        {
            var resultJson = await syncLogic.CallPhpProxyAsync("paymentStatusUpdate", extra_data);
            return Content(resultJson, "application/json");
        }
    }
}
