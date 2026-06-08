using Logic.Logic.ProductsLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Products;
using Models.Entities.Products;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/stock")]
    public class StockController : ControllerBase
    {
        private readonly IOvipStockLogic _stockLogic;

        public StockController(IOvipStockLogic stockLogic)
        {
            _stockLogic = stockLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _stockLogic.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var stock = await _stockLogic.GetByIdAsync(id);
            return stock == null ? NotFound() : Ok(stock);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            return Ok(await _stockLogic.GetByProductIdAsync(productId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OvipStockCreateDto stock)
        {
            return Ok(await _stockLogic.CreateAsync(stock));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OvipStockUpdateDto stock)
        {
            if (id != stock.Id)
                return BadRequest("Stock id in route does not match payload.");

            var updated = await _stockLogic.UpdateAsync(stock);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _stockLogic.DeleteAsync(id));
        }
    }
}
