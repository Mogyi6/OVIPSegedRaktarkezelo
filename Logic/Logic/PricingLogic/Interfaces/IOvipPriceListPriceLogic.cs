using Models.Dtos.Pircing;
using Models.Entities.Pricing;

namespace Logic.Logic.PricingLogic.Interfaces
{
    public interface IOvipPriceListPriceLogic
    {
        Task<List<OvipPriceListPrice>> GetAllAsync();

        Task<OvipPriceListPrice?> GetByIdAsync(int id);

        Task<List<OvipPriceListPrice>> GetByPriceListIdAsync(int priceListId);

        Task<List<OvipPriceListPrice>> GetByProductIdAsync(int productId);

        Task<OvipPriceListPrice> CreateAsync(OvipPriceListPriceCreateDto dto);

        Task<OvipPriceListPrice?> UpdateAsync(OvipPriceListPriceUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}