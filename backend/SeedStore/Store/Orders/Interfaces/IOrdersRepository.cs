using SeedStore.Database.Entities.Store.Orders;
using SeedStore.Store.Orders.Models;

namespace SeedStore.Store.Orders.Interfaces
{
    public interface IOrdersRepository
    {
        Task<List<AccountOrdersResponseModel>> GetAccountOrdersAsync(int accountId);
        Task AddOrderAsync(OrderEntity order);
        Task<Dictionary<int, (string name, string? imageUrl)>> GetProductSnapshotsAsync(List<int> productIds);

        Task<OrderEntity?> GetOrderByNumberAsync(string orderNumber);
        Task AddTransactionAsync(OrderTransactionEntity transaction);
        Task UpdateOrderAsync(OrderEntity order);

        Task ReserveProductQuantitiesAsync(List<(int productId, int quantity)> items);
    }
}