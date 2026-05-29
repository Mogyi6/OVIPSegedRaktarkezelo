using Models.Dtos.Products;
using Models.Entities.Products;

namespace Logic.Logic.ProductsLogic.Interfaces
{
    public interface IOvipProductLogic
    {
        Task<OvipProduct> CreateAsync(OvipProductCreateDto dto);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipProduct>> GetAllAsync();
        Task<OvipProduct?> GetByIdAsync(int id);
        Task<OvipProduct?> UpdateAsync(OvipProductUpdateDto dto);
    }
}