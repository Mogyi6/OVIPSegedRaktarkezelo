using Models.Entities.Products;

namespace Repository.Repository.ProductsRepository.Interfaces
{
    public interface IOvipStockRepository
    {
        Task<OvipStock> CreateAsync(OvipStock stock);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipStock>> GetAllAsync();
        Task<OvipStock?> GetByIdAsync(int id);
        Task<OvipStock?> GetByProductIdAsync(int productId);
        Task<OvipStock?> UpdateAsync(OvipStock stock);
    }
}