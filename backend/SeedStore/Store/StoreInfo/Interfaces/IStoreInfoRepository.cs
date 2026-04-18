using SeedStore.Database.Entities.Store.StoreInfo;

namespace SeedStore.Store.StoreInfo.Interfaces
{
    public interface IStoreInfoRepository
    {
        Task<List<DeliveryVariantEntity>> GetDeliveryVariantsAsync();
        Task<List<PaymentVariantEntity>> GetPaymentVariantsAsync();
        Task<AboutPageEntity?> GetAboutPageAsync();
        Task<ContactsEntity?> GetContactsAsync();
    }
}
