using Models.Entities.Products;

namespace Repository.Repository.ProductsRepository.Interfaces
{
    public interface IOvipProductParameterRepository
    {
        Task<OvipProductParameter> CreateAsync(OvipProductParameter parameter);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipProductParameter>> GetAllAsync();
        Task<OvipProductParameter?> GetByIdAsync(int id);
        Task<List<OvipProductParameter>> GetByProductIdAsync(int productId);
        Task<OvipProductParameter?> UpdateAsync(OvipProductParameter parameter);
    }
}