using Models.Entities.Parameters;

namespace Repository.Repository.ParametersRepositroy.Interfaces
{
    public interface IOvipParameterRepository
    {
        Task<OvipParameter> CreateAsync(OvipParameter entity);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipParameter>> GetAllAsync();
        Task<OvipParameter?> GetByIdAsync(int id);
        Task<OvipParameter?> UpdateAsync(OvipParameter entity);
    }
}