using System.Threading.Tasks;

namespace Logic.Logic.Sync
{
    public interface IOvipSyncLogic
    {
        Task SyncAllAsync();

        Task<string> SyncCategoriesAsync();
        Task<string> SyncParametersAsync();
        Task<string> SyncPriceListsAsync();
        Task<string> SyncProductsAsync(string? extraData = null, int? limitFrom = null, int? limitTo = null);
        Task<string> SyncCategoryConnectionsAsync();
        Task<string> SyncPriceListPricesAsync();
        Task<string> SyncQuantityDiscountsAsync();
        Task<string> SyncManufacturesAsync();
        Task<string> CallPhpProxyAsync(string request, string? extraData = null, int? limitFrom = null, int? limitTo = null);
    }
}
