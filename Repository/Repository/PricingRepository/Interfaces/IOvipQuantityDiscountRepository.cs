using Models.Entities.Pricing;

namespace Repository.Repository.PricingRepository.Interfaces
{
    public interface IOvipQuantityDiscountRepository
    {
        Task<OvipQuantityDiscount> CreateAsync(OvipQuantityDiscount entity);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipQuantityDiscount>> GetAllAsync();
        Task<OvipQuantityDiscount?> GetByIdAsync(int id);
        Task<List<OvipQuantityDiscount>> GetByPriceListIdAsync(int priceListId);
        Task<List<OvipQuantityDiscount>> GetByProductIdAsync(int productId);
        Task<OvipQuantityDiscount?> UpdateAsync(OvipQuantityDiscount entity);
    }
}