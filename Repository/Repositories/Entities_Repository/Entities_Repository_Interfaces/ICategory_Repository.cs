using Models.Entities;

namespace Repository.Repositories.Entities_Repository.Entities_Repository_Interfaces
{
    public interface ICategory_Repository
    {
        Task<Category> CreateAsync(Category category);
        Task<bool> DeleteAsync(int id);
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<bool> UpdateAsync(Category category);
    }
}