using Models.Entities.Manufacture;

namespace Repository.Repository.ManufactureRepositroy.Interfaces
{
    public interface IOvipManufactureRepository
    {
        Task<OvipManufacture> CreateAsync(OvipManufacture entity);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipManufacture>> GetAllAsync();
        Task<OvipManufacture?> GetByIdAsync(int id);
        Task<OvipManufacture?> GetByProductIdAsync(int productId);
        Task<OvipManufacture?> UpdateAsync(OvipManufacture entity);
    }
}