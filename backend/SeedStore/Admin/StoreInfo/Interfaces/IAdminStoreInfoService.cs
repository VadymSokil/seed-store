using SeedStore.Admin.StoreInfo.Models;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.StoreInfo.Interfaces
{
    public interface IAdminStoreInfoService
    {
        Task<List<ResponseDeliveryVariantModel>> GetDeliveryVariantsAsync();
        Task<string> AddDeliveryVariantAsync(AddDeliveryVariantModel model, int initiatorId);
        Task<string> UpdateDeliveryVariantAsync(int id, UpdateDeliveryVariantModel model, int initiatorId);
        Task<string> DeleteDeliveryVariantAsync(int id, int initiatorId);

        Task<List<ResponsePaymentVariantModel>> GetPaymentVariantsAsync();
        Task<string> AddPaymentVariantAsync(AddPaymentVariantModel model, int initiatorId);
        Task<string> UpdatePaymentVariantAsync(int id, UpdatePaymentVariantModel model, int initiatorId);
        Task<string> DeletePaymentVariantAsync(int id, int initiatorId);

        Task ReorderDeliveryVariantsAsync(List<ReorderItemModel> items, int initiatorId);
        Task ReorderPaymentVariantsAsync(List<ReorderItemModel> items, int initiatorId);

        Task<string?> GetAboutPageAsync();
        Task<string> UpdateAboutPageAsync(UpdatePageContentModel model, int initiatorId);
        Task<string?> GetContactsAsync();
        Task<string> UpdateContactsAsync(UpdatePageContentModel model, int initiatorId);
    }
}
