using seed_store_api.Database.Entities.Store.Modules.Orders;
using seed_store_api.Store.Modules.Orders.Models;

namespace seed_store_api.Store.Modules.Orders.Interfaces
{
    public interface IOrdersRepository
    {
        Task<List<AccountOrdersResponseModel>> GetAccountOrdersAsync(int accountId);
        Task AddOrderAsync(OrderEntity order);
        Task<Dictionary<int, (string name, string? imageUrl)>> GetProductSnapshotsAsync(List<int> productIds);

        Task<OrderEntity?> GetOrderByNumberAsync(string orderNumber);
        Task AddTransactionAsync(OrderTransactionEntity transaction);
        Task UpdateOrderAsync(OrderEntity order);
    }
}