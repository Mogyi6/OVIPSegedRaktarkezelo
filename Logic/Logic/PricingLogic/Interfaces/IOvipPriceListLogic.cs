using Models.Dtos.Pircing;
using Models.Entities.Pricing;

namespace Logic.Logic.PricingLogic.Interfaces
{
    public interface IOvipPriceListLogic
    {
        Task<List<OvipPriceList>> GetAllAsync();

        Task<OvipPriceList?> GetByIdAsync(int id);

        Task<OvipPriceList> CreateAsync(OvipPriceListCreateDto dto);

        Task<OvipPriceList?> UpdateAsync(OvipPriceListUpdateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}