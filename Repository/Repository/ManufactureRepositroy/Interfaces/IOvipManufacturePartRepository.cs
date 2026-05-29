using Models.Entities.Manufacture;

namespace Repository.Repository.ManufactureRepositroy.Interfaces
{
    public interface IOvipManufacturePartRepository
    {
        Task<OvipManufacturePart> CreateAsync(OvipManufacturePart entity);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipManufacturePart>> GetAllAsync();
        Task<OvipManufacturePart?> GetByIdAsync(int id);
        Task<List<OvipManufacturePart>> GetByManufactureIdAsync(int manufactureId);
        Task<List<OvipManufacturePart>> GetByPartProductIdAsync(int productId);
        Task<OvipManufacturePart?> UpdateAsync(OvipManufacturePart entity);
    }
}