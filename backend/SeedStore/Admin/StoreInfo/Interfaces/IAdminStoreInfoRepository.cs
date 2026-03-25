using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Store.StoreInfo;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.StoreInfo.Interfaces
{
    public interface IAdminStoreInfoRepository
    {
        Task<List<DeliveryVariantEntity>> GetDeliveryVariantsAsync();
        Task<DeliveryVariantEntity?> GetDeliveryVariantByIdAsync(int id);
        Task<bool> DeliveryVariantCodeExistsAsync(string code);
        Task AddDeliveryVariantAsync(DeliveryVariantEntity entity);
        Task UpdateDeliveryVariantAsync(DeliveryVariantEntity entity);
        Task DeleteDeliveryVariantAsync(DeliveryVariantEntity entity);

        Task<List<PaymentVariantEntity>> GetPaymentVariantsAsync();
        Task<PaymentVariantEntity?> GetPaymentVariantByIdAsync(int id);
        Task<bool> PaymentVariantCodeExistsAsync(string code);
        Task AddPaymentVariantAsync(PaymentVariantEntity entity);
        Task UpdatePaymentVariantAsync(PaymentVariantEntity entity);
        Task DeletePaymentVariantAsync(PaymentVariantEntity entity);
        Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId);

        Task ReorderDeliveryVariantsAsync(List<ReorderItemModel> items);
        Task ReorderPaymentVariantsAsync(List<ReorderItemModel> items);

        Task<List<ReorderItemModel>> GetDeliveryVariantsOrderAsync();
        Task<List<ReorderItemModel>> GetPaymentVariantsOrderAsync();
    }
}
