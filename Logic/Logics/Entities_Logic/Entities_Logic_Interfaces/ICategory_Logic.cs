using Models.Entities;

namespace Logic.Logics.Entities_Logic.Entities_Logic_Interfaces
{
    public interface ICategory_Logic
    {
        Task<Category?> CreateAsync(Category category);
        Task<bool> DeleteAsync(int id);
        Task<List<Category>?> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Category category);
    }
}