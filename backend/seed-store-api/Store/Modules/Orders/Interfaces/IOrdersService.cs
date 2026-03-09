using seed_store_api.Store.Modules.Orders.Models;

namespace seed_store_api.Store.Modules.Orders.Interfaces
{
    public interface IOrdersService
    {
        Task<(string status, List<AccountOrdersResponseModel>? orders)> GetAccountOrdersAsync(int accountId);
        Task<(string status, object? data)> AddOrderAsync(AddOrderModel model, int? accountId);
        Task<string> ProcessPaymentCallbackAsync(PaymentCallbackModel model);
    }
}