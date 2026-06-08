using Logic.Logic.CategoriesLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Categories;
using Models.Entities.Categories;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IOvipCategoryLogic _categoryLogic;

        public CategoriesController(IOvipCategoryLogic categoryLogic)
        {
            _categoryLogic = categoryLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _categoryLogic.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryLogic.GetByIdAsync(id);
            return category == null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OvipCategoryCreateDto category)
        {
            return Ok(await _categoryLogic.CreateAsync(category));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OvipCategoryUpdateDto category)
        {
            if (id != category.OvipCategoryId)
                return BadRequest("Category id in route does not match payload.");

            var updated = await _categoryLogic.UpdateAsync(category);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _categoryLogic.DeleteAsync(id));
        }
    }
}
