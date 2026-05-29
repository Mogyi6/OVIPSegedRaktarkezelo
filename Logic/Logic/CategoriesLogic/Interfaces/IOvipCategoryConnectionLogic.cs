using Models.Dtos.Categories;
using Models.Entities.Categories;

namespace Logic.Logic.CategoriesLogic.Interfaces
{
    public interface IOvipCategoryConnectionLogic
    {
        Task<OvipCategoryConnection> CreateAsync(OvipCategoryConnectionCreateDto entity);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipCategoryConnection>> GetAllAsync();
        //Task<List<OvipCategoryConnection>> GetByCategoryIdAsync(int categoryId);
        Task<OvipCategoryConnection?> GetByIdAsync(int id);
        //Task<List<OvipCategoryConnection>> GetByProductIdAsync(int productId);
        Task<OvipCategoryConnection?> UpdateAsync(OvipCategoryConnectionUpdateDto entity);
    }
}