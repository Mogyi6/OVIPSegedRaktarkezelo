using Models.Entities.Pricing;

namespace Repository.Repository.PricingRepository.Interfaces
{
    public interface IOvipPriceListPriceRepository
    {
        Task<OvipPriceListPrice> CreateAsync(OvipPriceListPrice entity);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipPriceListPrice>> GetAllAsync();
        Task<OvipPriceListPrice?> GetByIdAsync(int id);
        Task<List<OvipPriceListPrice>> GetByPriceListIdAsync(int priceListId);
        Task<List<OvipPriceListPrice>> GetByProductIdAsync(int productId);
        Task<OvipPriceListPrice?> UpdateAsync(OvipPriceListPrice entity);
    }
}