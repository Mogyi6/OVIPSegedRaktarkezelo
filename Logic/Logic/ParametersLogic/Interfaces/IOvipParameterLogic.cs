using Models.Dtos.Parameters;
using Models.Entities.Parameters;

namespace Logic.Logic.ParametersLogic.Interfaces
{
    public interface IOvipParameterLogic
    {
        Task<List<OvipParameter>> GetAllAsync();

        Task<OvipParameter?> GetByIdAsync(int id);

        Task<OvipParameter> CreateAsync(OvipParameterCreateDto dto);

        Task<OvipParameter?> UpdateAsync(OvipParameterUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}