using Models.Dtos.Manufacture;
using Models.Entities.Manufacture;

namespace Logic.Logic.ManufactureLogic.Interfaces
{
    public interface IOvipManufactureLogic
    {
        Task<List<OvipManufacture>> GetAllAsync();

        Task<OvipManufacture?> GetByIdAsync(int id);

        Task<OvipManufacture?> GetByProductIdAsync(int productId);

        Task<OvipManufacture> CreateAsync(OvipManufactureCreateDto dto);

        Task<OvipManufacture?> UpdateAsync(OvipManufactureUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}