using Logic.Logic.PricingLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Pircing;
using Models.Entities.Pricing;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/prices")]
    public class PriceListPricesController : ControllerBase
    {
        private readonly IOvipPriceListPriceLogic _priceListPriceLogic;

        public PriceListPricesController(IOvipPriceListPriceLogic priceListPriceLogic)
        {
            _priceListPriceLogic = priceListPriceLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _priceListPriceLogic.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var price = await _priceListPriceLogic.GetByIdAsync(id);
            return price == null ? NotFound() : Ok(price);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            return Ok(await _priceListPriceLogic.GetByProductIdAsync(productId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OvipPriceListPriceCreateDto price)
        {
            return Ok(await _priceListPriceLogic.CreateAsync(price));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OvipPriceListPriceUpdateDto price)
        {
            if (id != price.Id)
                return BadRequest("Price id in route does not match payload.");

            var updated = await _priceListPriceLogic.UpdateAsync(price);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _priceListPriceLogic.DeleteAsync(id));
        }
    }
}
