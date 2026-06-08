using Logic.Logic.PricingLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Pircing;
using Models.Entities.Pricing;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/pricelists")]
    public class PriceListsController : ControllerBase
    {
        private readonly IOvipPriceListLogic _priceListLogic;

        public PriceListsController(IOvipPriceListLogic priceListLogic)
        {
            _priceListLogic = priceListLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _priceListLogic.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var priceList = await _priceListLogic.GetByIdAsync(id);
            return priceList == null ? NotFound() : Ok(priceList);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OvipPriceListCreateDto priceList)
        {
            return Ok(await _priceListLogic.CreateAsync(priceList));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OvipPriceListUpdateDto priceList)
        {
            if (id != priceList.OvipPriceListId)
                return BadRequest("Price list id in route does not match payload.");

            var updated = await _priceListLogic.UpdateAsync(priceList);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _priceListLogic.DeleteAsync(id));
        }
    }
}
