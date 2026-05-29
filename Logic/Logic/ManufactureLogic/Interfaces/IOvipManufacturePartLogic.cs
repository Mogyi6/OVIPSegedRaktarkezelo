using Models.Dtos.Manufacture;
using Models.Entities.Manufacture;

namespace Logic.Logic.ManufactureLogic.Interfaces
{
    public interface IOvipManufacturePartLogic
    {
        Task<List<OvipManufacturePart>> GetAllAsync();

        Task<OvipManufacturePart?> GetByIdAsync(int id);

        Task<List<OvipManufacturePart>> GetByManufactureIdAsync(int manufactureId);

        Task<List<OvipManufacturePart>> GetByPartProductIdAsync(int productId);

        Task<OvipManufacturePart> CreateAsync(OvipManufacturePartCreateDto entity);

        Task<OvipManufacturePart?> UpdateAsync(OvipManufacturePartUpdateDto entity);

        Task<bool> DeleteAsync(int id);
    }
}