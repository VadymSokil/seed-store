using SeedStore.Store.Orders.Models;
using SeedStore.Support.Store.Dictionary.NovaPoshtaDelivery.Models;

namespace SeedStore.Store.Orders.Interfaces
{
    public interface IOrdersService
    {
        Task<(string status, AccountOrdersResponseModel? data)> GetAccountOrdersAsync(int accountId, int page, int pageSize);
        Task<(string status, object? data)> AddOrderAsync(AddOrderModel model, int? accountId);
        Task<string> ProcessPaymentCallbackAsync(PaymentCallbackModel model);
        Task<(string status, object? data)> GetPaymentDataAsync(string orderNumber, int accountId);

        Task<List<NovaPoshtaSettlementModel>> SearchSettlementsAsync(string value);
        Task<List<NovaPoshtaWarehouseModel>> SearchWarehousesAsync(string settlementId, string? value);
    }
}