using Logic.Logic.PricingLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Pircing;
using Models.Entities.Pricing;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/discounts")]
    public class QuantityDiscountsController : ControllerBase
    {
        private readonly IOvipQuantityDiscountLogic _quantityDiscountLogic;

        public QuantityDiscountsController(IOvipQuantityDiscountLogic quantityDiscountLogic)
        {
            _quantityDiscountLogic = quantityDiscountLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _quantityDiscountLogic.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var discount = await _quantityDiscountLogic.GetByIdAsync(id);
            return discount == null ? NotFound() : Ok(discount);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            return Ok(await _quantityDiscountLogic.GetByProductIdAsync(productId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OvipQuantityDiscountCreateDto discount)
        {
            return Ok(await _quantityDiscountLogic.CreateAsync(discount));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OvipQuantityDiscountUpdateDto discount)
        {
            if (id != discount.OvipQuantityId)
                return BadRequest("Discount id in route does not match payload.");

            var updated = await _quantityDiscountLogic.UpdateAsync(discount);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _quantityDiscountLogic.DeleteAsync(id));
        }
    }
}
