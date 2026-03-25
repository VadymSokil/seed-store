using SeedStore.Store.StoreInfo.Models;

namespace SeedStore.Store.StoreInfo.Interfaces
{
    public interface IStoreInfoService
    {
        Task<List<VariantResponseModel>> GetDeliveryVariantsAsync();
        Task<List<VariantResponseModel>> GetPaymentVariantsAsync();
    }
}
