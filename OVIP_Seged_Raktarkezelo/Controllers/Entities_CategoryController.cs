using Logic.Logics.Entities_Logic.Entities_Logic_Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Entities_CategoryController : ControllerBase
    {
        private readonly ICategory_Logic _categoryLogic;

        public Entities_CategoryController(ICategory_Logic categoryLogic)
        {
            _categoryLogic = categoryLogic;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAll()
        {
            var result = await _categoryLogic.GetAllAsync();
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var result = await _categoryLogic.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Category>> Create([FromBody] Category category)
        {
            var result = await _categoryLogic.CreateAsync(category);
            if (result == null)
                return BadRequest();

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Category category)
        {
            if (id != category.OvipCategoryId)
                return BadRequest("ID mismatch");

            var success = await _categoryLogic.UpdateAsync(category);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _categoryLogic.DeleteAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
