using Logic.Logic.ManufactureLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Manufacture;
using Models.Entities.Manufacture;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/manufacture-parts")]
    public class ManufacturePartsController : ControllerBase
    {
        private readonly IOvipManufacturePartLogic _manufacturePartLogic;

        public ManufacturePartsController(IOvipManufacturePartLogic manufacturePartLogic)
        {
            _manufacturePartLogic = manufacturePartLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _manufacturePartLogic.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var part = await _manufacturePartLogic.GetByIdAsync(id);
            return part == null ? NotFound() : Ok(part);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OvipManufacturePartCreateDto part)
        {
            return Ok(await _manufacturePartLogic.CreateAsync(part));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OvipManufacturePartUpdateDto part)
        {
            if (id != part.Id)
                return BadRequest("Manufacture part id in route does not match payload.");

            var updated = await _manufacturePartLogic.UpdateAsync(part);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _manufacturePartLogic.DeleteAsync(id));
        }
    }
}
