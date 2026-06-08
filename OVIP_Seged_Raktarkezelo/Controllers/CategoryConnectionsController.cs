using Logic.Logic.CategoriesLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Categories;
using Models.Entities.Categories;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/category-connections")]
    public class CategoryConnectionsController : ControllerBase
    {
        private readonly IOvipCategoryConnectionLogic _categoryConnectionLogic;

        public CategoryConnectionsController(IOvipCategoryConnectionLogic categoryConnectionLogic)
        {
            _categoryConnectionLogic = categoryConnectionLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _categoryConnectionLogic.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var connection = await _categoryConnectionLogic.GetByIdAsync(id);
            return connection == null ? NotFound() : Ok(connection);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OvipCategoryConnectionCreateDto connection)
        {
            return Ok(await _categoryConnectionLogic.CreateAsync(connection));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OvipCategoryConnectionUpdateDto connection)
        {
            if (id != connection.Id)
                return BadRequest("Connection id in route does not match payload.");

            var updated = await _categoryConnectionLogic.UpdateAsync(connection);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _categoryConnectionLogic.DeleteAsync(id));
        }
    }
}
