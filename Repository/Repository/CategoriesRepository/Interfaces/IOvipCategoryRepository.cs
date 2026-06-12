using Models.Entities;
using Models.Entities.Categories;

namespace Repository.Repository.CategoriesRepository.Interfaces
{
    public interface IOvipCategoryRepository
    {
        Task<Category> CreateAsync(Category entity);
        Task<bool> DeleteAsync(int id);
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> UpdateAsync(Category entity);
    }
}