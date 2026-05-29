using Models.Dtos.Products;
using Models.Entities.Products;

namespace Logic.Logic.ProductsLogic.Interfaces
{
    public interface IOvipProductImageLogic
    {
        Task<List<OvipProductImage>> GetAllAsync();

        Task<OvipProductImage?> GetByIdAsync(int id);

        Task<List<OvipProductImage>> GetByProductIdAsync(int productId);

        Task<OvipProductImage> CreateAsync(OvipProductImageCreateDto dto);

        Task<OvipProductImage?> UpdateAsync(OvipProductImageUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}