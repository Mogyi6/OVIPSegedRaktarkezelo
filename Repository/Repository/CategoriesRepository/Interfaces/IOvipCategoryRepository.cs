using Models.Entities.Categories;

namespace Repository.Repository.CategoriesRepository.Interfaces
{
    public interface IOvipCategoryRepository
    {
        Task<OvipCategory> CreateAsync(OvipCategory entity);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipCategory>> GetAllAsync();
        Task<OvipCategory?> GetByIdAsync(int id);
        Task<OvipCategory?> UpdateAsync(OvipCategory entity);
    }
}