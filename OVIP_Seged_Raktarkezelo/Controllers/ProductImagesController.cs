using Logic.Logic.ProductsLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Products;
using Models.Entities.Products;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/images")]
    public class ProductImagesController : ControllerBase
    {
        private readonly IOvipProductImageLogic _imageLogic;

        public ProductImagesController(IOvipProductImageLogic imageLogic)
        {
            _imageLogic = imageLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _imageLogic.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var image = await _imageLogic.GetByIdAsync(id);
            return image == null ? NotFound() : Ok(image);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            return Ok(await _imageLogic.GetByProductIdAsync(productId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OvipProductImageCreateDto image)
        {
            return Ok(await _imageLogic.CreateAsync(image));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OvipProductImageUpdateDto image)
        {
            if (id != image.Id)
                return BadRequest("Image id in route does not match payload.");

            var updated = await _imageLogic.UpdateAsync(image);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _imageLogic.DeleteAsync(id));
        }
    }
}
