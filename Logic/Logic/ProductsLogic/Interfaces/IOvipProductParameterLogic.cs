using Models.Dtos.Products;
using Models.Entities.Products;

namespace Logic.Logic.ProductsLogic.Interfaces
{
    public interface IOvipProductParameterLogic
    {
        Task<List<OvipProductParameter>> GetAllAsync();

        Task<OvipProductParameter?> GetByIdAsync(int id);

        Task<List<OvipProductParameter>> GetByProductIdAsync(int productId);

        Task<OvipProductParameter> CreateAsync(OvipProductParameterCreateDto dto);

        Task<OvipProductParameter?> UpdateAsync(OvipProductParameterUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}