using SeedStore.Store.Orders.Models;

namespace SeedStore.Store.Orders.Interfaces
{
    public interface IOrdersService
    {
        Task<(string status, List<AccountOrdersResponseModel>? orders)> GetAccountOrdersAsync(int accountId);
        Task<(string status, object? data)> AddOrderAsync(AddOrderModel model, int? accountId);
        Task<string> ProcessPaymentCallbackAsync(PaymentCallbackModel model);
    }
}