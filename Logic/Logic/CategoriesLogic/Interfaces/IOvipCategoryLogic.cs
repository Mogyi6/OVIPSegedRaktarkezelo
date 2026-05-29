using Models.Dtos.Categories;
using Models.Entities.Categories;

namespace Logic.Logic.CategoriesLogic.Interfaces
{
    public interface IOvipCategoryLogic
    {
        Task<OvipCategory> CreateAsync(OvipCategoryCreateDto entity);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipCategory>> GetAllAsync();
        Task<OvipCategory?> GetByIdAsync(int id);
        Task<OvipCategory?> UpdateAsync(OvipCategoryUpdateDto entity);
    }
}