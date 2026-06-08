using Logic.Logic.ProductsLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Products;
using Models.Entities.Products;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IOvipProductLogic _productLogic;

        public ProductsController(IOvipProductLogic productLogic)
        {
            _productLogic = productLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _productLogic.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productLogic.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OvipProductCreateDto product)
        {
            return Ok(await _productLogic.CreateAsync(product));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OvipProductUpdateDto product)
        {
            if (id != product.OvipProductId)
                return BadRequest("Product id in route does not match payload.");

            var updated = await _productLogic.UpdateAsync(product);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _productLogic.DeleteAsync(id));
        }
    }
}
