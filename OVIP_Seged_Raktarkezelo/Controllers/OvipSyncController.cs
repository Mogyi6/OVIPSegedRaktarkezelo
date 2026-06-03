using Logic.Logic.SOAPClient;
using Microsoft.AspNetCore.Mvc;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/ovip-sync")]
    public class OvipSyncController : ControllerBase
    {
        private readonly IOvipSyncLogic _ovipSyncLogic;

        public OvipSyncController(IOvipSyncLogic ovipSyncLogic)
        {
            _ovipSyncLogic = ovipSyncLogic;
        }

        [HttpPost("all")]
        public async Task<IActionResult> SyncAll()
        {
            try
            {
                await _ovipSyncLogic.SyncAllAsync();

                return Ok(new
                {
                    message = "OVIP teljes szinkron sikeresen lefutott."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "OVIP szinkron hiba történt.",
                    error = ex.Message,
                    exception = ex.GetType().Name
                });
            }
        }
    }
}