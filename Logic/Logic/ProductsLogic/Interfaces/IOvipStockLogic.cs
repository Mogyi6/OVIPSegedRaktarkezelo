using Models.Dtos.Products;
using Models.Entities.Products;

namespace Logic.Logic.ProductsLogic.Interfaces
{
    public interface IOvipStockLogic
    {
        Task<List<OvipStock>> GetAllAsync();

        Task<OvipStock?> GetByIdAsync(int id);

        Task<OvipStock?> GetByProductIdAsync(int productId);

        Task<OvipStock> CreateAsync(OvipStockCreateDto dto);

        Task<OvipStock?> UpdateAsync(OvipStockUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}