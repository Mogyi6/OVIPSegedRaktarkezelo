using Logic.Logic.ParametersLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos.Parameters;
using Models.Entities.Parameters;

namespace OVIP_Seged_Raktarkezelo.Controllers
{
    [ApiController]
    [Route("api/parameters")]
    public class ParametersController : ControllerBase
    {
        private readonly IOvipParameterLogic _parameterLogic;

        public ParametersController(IOvipParameterLogic parameterLogic)
        {
            _parameterLogic = parameterLogic;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _parameterLogic.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var parameter = await _parameterLogic.GetByIdAsync(id);
            return parameter == null ? NotFound() : Ok(parameter);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OvipParameterCreateDto parameter)
        {
            return Ok(await _parameterLogic.CreateAsync(parameter));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] OvipParameterUpdateDto parameter)
        {
            if (id != parameter.OvipParameterId)
                return BadRequest("Parameter id in route does not match payload.");

            var updated = await _parameterLogic.UpdateAsync(parameter);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _parameterLogic.DeleteAsync(id));
        }
    }
}
