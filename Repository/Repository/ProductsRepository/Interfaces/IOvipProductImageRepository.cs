using Models.Entities.Products;

namespace Repository.Repository.ProductsRepository.Interfaces
{
    public interface IOvipProductImageRepository
    {
        Task<OvipProductImage> CreateAsync(OvipProductImage image);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipProductImage>> GetAllAsync();
        Task<OvipProductImage?> GetByIdAsync(int id);
        Task<List<OvipProductImage>> GetByProductIdAsync(int productId);
        Task<OvipProductImage?> UpdateAsync(OvipProductImage image);
    }
}