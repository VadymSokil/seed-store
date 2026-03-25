using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Admin.Orders;
using SeedStore.Database.Entities.Store.Orders;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Orders.Interfaces
{
    public interface IAdminOrdersRepository
    {
        Task<List<OrderEntity>> GetOrdersAsync(string? statusCode);
        Task<OrderEntity?> GetOrderByIdAsync(int orderId);
        Task<OrderItemEntity?> GetOrderItemByIdAsync(int itemId);
        Task UpdateOrderAsync(OrderEntity order);
        Task UpdateOrderItemAsync(OrderItemEntity item);
        Task AddOrderProcessingAsync(OrderProcessingEntity processing);
        Task<List<OrderActionEntity>> GetOrderActionsAsync();
        Task<OrderActionEntity?> GetOrderActionByIdAsync(int actionId);
        Task AddOrderActionAsync(OrderActionEntity action);
        Task UpdateOrderActionAsync(OrderActionEntity action);
        Task DeleteOrderActionAsync(OrderActionEntity action);

        Task<List<OrderStatusEntity>> GetOrderStatusesAsync();
        Task<OrderStatusEntity?> GetOrderStatusByIdAsync(int statusId);
        Task<bool> OrderStatusCodeExistsAsync(string code);
        Task AddOrderStatusAsync(OrderStatusEntity status);
        Task UpdateOrderStatusAsync(OrderStatusEntity status);
        Task DeleteOrderStatusAsync(OrderStatusEntity status);
        Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId);

        Task ReorderOrderStatusesAsync(List<ReorderItemModel> items);
        Task ReorderOrderActionsAsync(List<ReorderItemModel> items);

        Task<bool> OrderProcessingExistsAsync(int orderId, string actionCode);

        Task<List<OrderItemEntity>> GetOrderItemsAsync(int orderId);
        Task RestoreProductQuantitiesAsync(List<(int productId, int quantity)> items);

        Task<List<ReorderItemModel>> GetOrderStatusesOrderAsync();
        Task<List<ReorderItemModel>> GetOrderActionsOrderAsync();
    }
}
