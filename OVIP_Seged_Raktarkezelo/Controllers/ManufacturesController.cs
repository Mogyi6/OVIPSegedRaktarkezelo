using Logic.Logic.ManufactureLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Manufacture;
using Models.Entities.Manufacture;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/manufactures")]
    public class ManufacturesController : ControllerBase
    {
        private readonly IOvipManufactureLogic _manufactureLogic;

        public ManufacturesController(IOvipManufactureLogic manufactureLogic)
        {
            _manufactureLogic = manufactureLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _manufactureLogic.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var manufacture = await _manufactureLogic.GetByIdAsync(id);
            return manufacture == null ? NotFound() : Ok(manufacture);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            return Ok(await _manufactureLogic.GetByProductIdAsync(productId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OvipManufactureCreateDto manufacture)
        {
            return Ok(await _manufactureLogic.CreateAsync(manufacture));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OvipManufactureUpdateDto manufacture)
        {
            if (id != manufacture.Id)
                return BadRequest("Manufacture id in route does not match payload.");

            var updated = await _manufactureLogic.UpdateAsync(manufacture);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _manufactureLogic.DeleteAsync(id));
        }
    }
}
