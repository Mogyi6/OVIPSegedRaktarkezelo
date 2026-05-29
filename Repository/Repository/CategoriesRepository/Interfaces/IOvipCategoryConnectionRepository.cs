using Models.Entities.Categories;

namespace Repository.Repository.CategoriesRepository.Interfaces
{
    public interface IOvipCategoryConnectionRepository
    {
        Task<OvipCategoryConnection> CreateAsync(OvipCategoryConnection entity);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipCategoryConnection>> GetAllAsync();
        Task<List<OvipCategoryConnection>> GetByCategoryIdAsync(int categoryId);
        Task<OvipCategoryConnection?> GetByIdAsync(int id);
        Task<List<OvipCategoryConnection>> GetByProductIdAsync(int productId);
        Task<OvipCategoryConnection?> UpdateAsync(OvipCategoryConnection entity);
    }
}