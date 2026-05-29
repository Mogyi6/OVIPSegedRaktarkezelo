using Models.Dtos.Pircing;
using Models.Entities.Pricing;

namespace Logic.Logic.PricingLogic.Interfaces
{
    public interface IOvipQuantityDiscountLogic
    {
        Task<List<OvipQuantityDiscount>> GetAllAsync();

        Task<OvipQuantityDiscount?> GetByIdAsync(int id);

        Task<List<OvipQuantityDiscount>> GetByProductIdAsync(int productId);

        Task<List<OvipQuantityDiscount>> GetByPriceListIdAsync(int priceListId);

        Task<OvipQuantityDiscount> CreateAsync(OvipQuantityDiscountCreateDto dto);

        Task<OvipQuantityDiscount?> UpdateAsync(OvipQuantityDiscountUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}