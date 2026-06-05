// using Logic.Logic.Sync; (using fully-qualified type names below)
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.SOAPClient;
using System.Net.Http;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/ovip-sync")]
    public class OvipSyncController : ControllerBase
    {
        private readonly Logic.Logic.Sync.IOvipSyncLogic _ovipSyncLogic;
        private readonly OvipOptions _ovipOptions;

        public OvipSyncController(
            Logic.Logic.Sync.IOvipSyncLogic ovipSyncLogic,
            IOptions<OvipOptions> ovipOptions)
        {
            _ovipSyncLogic = ovipSyncLogic;
            _ovipOptions = ovipOptions.Value;
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