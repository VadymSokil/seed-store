using seed_store_api.Database.Entities.Store.Modules.StoreInfo;

namespace seed_store_api.Store.Modules.StoreInfo.Interfaces
{
    public interface IStoreInfoRepository
    {
        Task<List<DeliveryVariantEntity>> GetDeliveryVariantsAsync();
        Task<List<PaymentVariantEntity>> GetPaymentVariantsAsync();
    }
}
