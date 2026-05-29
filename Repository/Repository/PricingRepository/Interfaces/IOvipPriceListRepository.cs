using Models.Entities.Pricing;

namespace Repository.Repository.PricingRepository.Interfaces
{
    public interface IOvipPriceListRepository
    {
        Task<OvipPriceList> CreateAsync(OvipPriceList priceList);
        Task<bool> DeleteAsync(int id);
        Task<List<OvipPriceList>> GetAllAsync();
        Task<OvipPriceList?> GetByIdAsync(int id);
        Task<OvipPriceList?> UpdateAsync(OvipPriceList priceList);
    }
}