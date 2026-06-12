using Models.Dtos.Categories;
using Models.Entities;
using Models.Entities.Categories;

namespace Logic.Logic.CategoriesLogic.Interfaces
{
    public interface IOvipCategoryLogic
    {
        Task<Category> CreateAsync(OvipCategoryCreateDto entity);
        Task<bool> DeleteAsync(int id);
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category?> UpdateAsync(OvipCategoryUpdateDto entity);
    }
}