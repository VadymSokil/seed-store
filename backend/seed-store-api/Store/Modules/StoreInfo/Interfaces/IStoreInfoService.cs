using seed_store_api.Store.Modules.StoreInfo.Models;

namespace seed_store_api.Store.Modules.StoreInfo.Interfaces
{
    public interface IStoreInfoService
    {
        Task<List<VariantResponseModel>> GetDeliveryVariantsAsync();
        Task<List<VariantResponseModel>> GetPaymentVariantsAsync();
    }
}
