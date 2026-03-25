using SeedStore.Admin.Orders.Models;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Orders.Interfaces
{
    public interface IAdminOrdersService
    {
        Task<List<OrdersResponseModel>> GetOrdersAsync(string? statusCode);
        Task<OrderDetailsResponseModel?> GetOrderAsync(int orderId);
        Task<string> UpdateOrderAsync(int orderId, UpdateOrderModel model, int initiatorId);
        Task<string> UpdateOrderStatusAsync(int orderId, string statusCode, int employeeId);
        Task<string> UpdateTrackingNumberAsync(int orderId, string trackingNumber, int employeeId);

        Task<List<ResponseOrderActionsModel>> GetOrderActionsAsync();
        Task<string> AddOrderActionAsync(AddOrderActionModel model, int initiatorId);
        Task<string> UpdateOrderActionAsync(int actionId, UpdateOrderActionModel model, int initiatorId);
        Task<string> DeleteOrderActionAsync(int actionId, int initiatorId);

        Task<List<ResponseOrderStatusesModel>> GetOrderStatusesAsync();
        Task<string> AddOrderStatusAsync(AddOrderStatusModel model, int initiatorId);
        Task<string> UpdateOrderStatusAsync(int statusId, UpdateOrderStatusModel model, int initiatorId);
        Task<string> DeleteOrderStatusAsync(int statusId, int initiatorId);

        Task ReorderOrderStatusesAsync(List<ReorderItemModel> items, int initiatorId);
        Task ReorderOrderActionsAsync(List<ReorderItemModel> items, int initiatorId);

        Task<string> PackOrderAsync(int orderId, int employeeId);

        Task<string> LogCallAsync(int orderId, LogCallModel model, int employeeId);

        Task<string> TakeOrderAsync(int orderId, int employeeId);
        Task<string> ReleaseOrderAsync(int orderId, int employeeId);
        Task<string> ResetOrderAsync(int orderId, int initiatorId);
    }
}
