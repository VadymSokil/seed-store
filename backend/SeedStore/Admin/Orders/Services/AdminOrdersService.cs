using SeedStore.Admin.Orders.Interfaces;
using SeedStore.Admin.Orders.Models;
using SeedStore.Database.Entities.Admin.Orders;
using SeedStore.Database.Entities.Store.Orders;
using SeedStore.Support.Admin.EmployeesActivity.Interfaces;
using SeedStore.Support.Admin.Reorder.Models;
using SeedStore.Support.General.Constants.Admin;
using SeedStore.Support.General.Constants.Store;
using SeedStore.Support.Store.Notifications.Interfaces;
using System.Text.Json;

namespace SeedStore.Admin.Orders.Services
{
    public class AdminOrdersService : IAdminOrdersService
    {
        private readonly IAdminOrdersRepository _repository;
        private readonly IEmployeesActivityService _employeesActivityService;
        private readonly INotificationService _notificationService;

        public AdminOrdersService(IAdminOrdersRepository repository, IEmployeesActivityService employeesActivityService, INotificationService notificationService)
        {
            _repository = repository;
            _employeesActivityService = employeesActivityService;
            _notificationService = notificationService;
        }

        public async Task<List<OrdersResponseModel>> GetOrdersAsync(string? statusCode)
        {
            var orders = await _repository.GetOrdersAsync(statusCode);

            return orders.Select(o => new OrdersResponseModel
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                OrderDate = o.OrderDate,
                StatusCode = o.StatusCode,
                TotalAmount = o.TotalAmount,
                FirstName = o.FirstName,
                LastName = o.LastName,
                PhoneNumber = o.PhoneNumber,
                DeliveryCode = o.DeliveryCode,
                PaymentCode = o.PaymentCode
            }).ToList();
        }

        public async Task<OrderDetailsResponseModel?> GetOrderAsync(int orderId)
        {
            var order = await _repository.GetOrderByIdAsync(orderId);

            if (order == null)
                return null;

            return new OrderDetailsResponseModel
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                StatusCode = order.StatusCode,
                Comment = order.Comment,
                DeliveryCode = order.DeliveryCode,
                PaymentCode = order.PaymentCode,
                PostalOfficeNumber = order.PostalOfficeNumber,
                TrackingNumber = order.TrackingNumber,
                TotalAmount = order.TotalAmount,
                FirstName = order.FirstName,
                LastName = order.LastName,
                MiddleName = order.MiddleName,
                PhoneNumber = order.PhoneNumber,
                Region = order.Region,
                District = order.District,
                City = order.City,
                Settlement = order.Settlement,
                Street = order.Street,
                HouseNumber = order.HouseNumber,
                ApartmentNumber = order.ApartmentNumber,
                PaidAt = order.PaidAt,
                Items = order.Items.Select(i => new OrderItemResponseModel
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductNameSnapshot = i.ProductNameSnapshot,
                    ProductImageUrlSnapshot = i.ProductImageUrlSnapshot,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };
        }

        public async Task<string> UpdateOrderAsync(int orderId, UpdateOrderModel model, int initiatorId)
        {
            var order = await _repository.GetOrderByIdAsync(orderId);

            if (order == null)
                return "not_found";

            var before = JsonSerializer.Serialize(new { order.Comment, order.DeliveryCode, order.PaymentCode, order.PostalOfficeNumber, order.FirstName, order.LastName, order.MiddleName, order.PhoneNumber });

            order.Comment = model.Comment;
            order.DeliveryCode = model.DeliveryCode;
            order.PaymentCode = model.PaymentCode;
            order.PostalOfficeNumber = model.PostalOfficeNumber;
            order.FirstName = model.FirstName;
            order.LastName = model.LastName;
            order.MiddleName = model.MiddleName;
            order.PhoneNumber = model.PhoneNumber;
            order.Region = model.Region;
            order.District = model.District;
            order.City = model.City;
            order.Settlement = model.Settlement;
            order.Street = model.Street;
            order.HouseNumber = model.HouseNumber;
            order.ApartmentNumber = model.ApartmentNumber;

            foreach (var itemModel in model.Items)
            {
                var item = await _repository.GetOrderItemByIdAsync(itemModel.ItemId);

                if (item == null || item.OrderId != orderId)
                    continue;

                item.Quantity = itemModel.Quantity;
                await _repository.UpdateOrderItemAsync(item);
            }

            order.TotalAmount = order.Items.Sum(i => i.Price * i.Quantity);
            await _repository.UpdateOrderAsync(order);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = JsonSerializer.Serialize(new { model.Comment, model.DeliveryCode, model.PaymentCode, model.PostalOfficeNumber, model.FirstName, model.LastName, model.MiddleName, model.PhoneNumber });
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) дані замовлення {order.OrderNumber}.", before, after);

            return "ok";
        }

        public async Task<string> UpdateOrderStatusAsync(int orderId, string statusCode, int employeeId)
        {
            var order = await _repository.GetOrderByIdAsync(orderId);

            if (order == null)
                return "not_found";

            order.StatusCode = statusCode;

            if (statusCode == OrderStatusCodes.Cancelled)
            {
                var items = await _repository.GetOrderItemsAsync(orderId);
                await _repository.RestoreProductQuantitiesAsync(items.Select(i => (i.ProductId, i.Quantity)).ToList());
            }

            await _repository.UpdateOrderAsync(order);

            var initiator = await _repository.GetEmployeeByIdAsync(employeeId);
            await _employeesActivityService.LogAsync(employeeId, $"{initiator!.Role!.Name} {initiator.Name}({employeeId}) змінив(ла) статус замовлення {order.OrderNumber} на {statusCode}.");

            return "ok";
        }

        public async Task<string> PackOrderAsync(int orderId, int employeeId)
        {
            var order = await _repository.GetOrderByIdAsync(orderId);

            if (order == null)
                return "not_found";

            var alreadyPacked = await _repository.OrderProcessingExistsAsync(orderId, OrderActionCodes.Packed);

            if (alreadyPacked)
                return "already_packed";

            await _repository.AddOrderProcessingAsync(new OrderProcessingEntity
            {
                OrderId = orderId,
                EmployeeId = employeeId,
                ActionCode = OrderActionCodes.Packed,
                CreatedAt = DateTime.UtcNow
            });

            var initiator = await _repository.GetEmployeeByIdAsync(employeeId);
            await _employeesActivityService.LogAsync(employeeId, $"{initiator!.Role!.Name} {initiator.Name}({employeeId}) упакував(ла) замовлення {order.OrderNumber}.");

            return "ok";
        }

        public async Task<string> UpdateTrackingNumberAsync(int orderId, string trackingNumber, int employeeId)
        {
            var order = await _repository.GetOrderByIdAsync(orderId);

            if (order == null)
                return "not_found";

            var isPacked = await _repository.OrderProcessingExistsAsync(orderId, OrderActionCodes.Packed);
            if (!isPacked)
                return "not_packed";

            order.TrackingNumber = trackingNumber;
            await _repository.UpdateOrderAsync(order);

            await _repository.AddOrderProcessingAsync(new OrderProcessingEntity
            {
                OrderId = orderId,
                EmployeeId = employeeId,
                ActionCode = OrderActionCodes.Shipped,
                CreatedAt = DateTime.UtcNow
            });

            var initiator = await _repository.GetEmployeeByIdAsync(employeeId);
            await _employeesActivityService.LogAsync(employeeId, $"{initiator!.Role!.Name} {initiator.Name}({employeeId}) встановив(ла) трекінг номер {trackingNumber} для замовлення {order.OrderNumber}.");

            return "ok";
        }

        public async Task<List<ResponseOrderActionsModel>> GetOrderActionsAsync()
        {
            var actions = await _repository.GetOrderActionsAsync();

            return actions.Select(a => new ResponseOrderActionsModel
            {
                Id = a.Id,
                Name = a.Name,
                IsActive = a.IsActive,
                ViewOrder = a.ViewOrder
            }).ToList();
        }

        public async Task<string> AddOrderActionAsync(AddOrderActionModel model, int initiatorId)
        {
            await _repository.AddOrderActionAsync(new OrderActionEntity
            {
                Name = model.Name,
                IsActive = model.IsActive,
                ViewOrder = model.ViewOrder
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) дію замовлення {model.Name}.");

            return "ok";
        }

        public async Task<string> UpdateOrderActionAsync(int actionId, UpdateOrderActionModel model, int initiatorId)
        {
            var action = await _repository.GetOrderActionByIdAsync(actionId);

            if (action == null)
                return "not_found";

            var before = JsonSerializer.Serialize(new { action.Name, action.IsActive, action.ViewOrder });

            action.Name = model.Name;
            action.IsActive = model.IsActive;
            action.ViewOrder = model.ViewOrder;
            await _repository.UpdateOrderActionAsync(action);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = JsonSerializer.Serialize(new { model.Name, model.IsActive, model.ViewOrder });
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) дію замовлення {action.Name}.", before, after);

            return "ok";
        }

        public async Task<string> DeleteOrderActionAsync(int actionId, int initiatorId)
        {
            var action = await _repository.GetOrderActionByIdAsync(actionId);

            if (action == null)
                return "not_found";

            await _repository.DeleteOrderActionAsync(action);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) дію замовлення {action.Name}.");

            return "ok";
        }

        public async Task<List<ResponseOrderStatusesModel>> GetOrderStatusesAsync()
        {
            var statuses = await _repository.GetOrderStatusesAsync();

            return statuses.Select(s => new ResponseOrderStatusesModel
            {
                Id = s.Id,
                Code = s.Code,
                Name = s.Name,
                IsActive = s.IsActive,
                ViewOrder = s.ViewOrder
            }).ToList();
        }

        public async Task<string> AddOrderStatusAsync(AddOrderStatusModel model, int initiatorId)
        {
            var codeExists = await _repository.OrderStatusCodeExistsAsync(model.Code);

            if (codeExists)
                return "code_taken";

            await _repository.AddOrderStatusAsync(new OrderStatusEntity
            {
                Code = model.Code,
                Name = model.Name,
                IsActive = model.IsActive,
                ViewOrder = model.ViewOrder
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) статус замовлення {model.Name}.");

            return "ok";
        }

        public async Task<string> UpdateOrderStatusAsync(int statusId, UpdateOrderStatusModel model, int initiatorId)
        {
            var status = await _repository.GetOrderStatusByIdAsync(statusId);

            if (status == null)
                return "not_found";

            var codeExists = await _repository.OrderStatusCodeExistsAsync(model.Code);

            if (codeExists && status.Code != model.Code)
                return "code_taken";

            var before = JsonSerializer.Serialize(new { status.Code, status.Name, status.IsActive, status.ViewOrder });

            status.Code = model.Code;
            status.Name = model.Name;
            status.IsActive = model.IsActive;
            status.ViewOrder = model.ViewOrder;
            await _repository.UpdateOrderStatusAsync(status);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = JsonSerializer.Serialize(new { model.Code, model.Name, model.IsActive, model.ViewOrder });
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) статус замовлення {status.Name}.", before, after);

            return "ok";
        }

        public async Task<string> DeleteOrderStatusAsync(int statusId, int initiatorId)
        {
            var status = await _repository.GetOrderStatusByIdAsync(statusId);

            if (status == null)
                return "not_found";

            await _repository.DeleteOrderStatusAsync(status);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) статус замовлення {status.Name}.");

            return "ok";
        }

        public async Task ReorderOrderStatusesAsync(List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetOrderStatusesOrderAsync();
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderOrderStatusesAsync(items);
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок статусів замовлень.", before, after);
        }

        public async Task ReorderOrderActionsAsync(List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetOrderActionsOrderAsync();
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderOrderActionsAsync(items);
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок дій замовлень.", before, after);
        }
        public async Task<string> LogCallAsync(int orderId, LogCallModel model, int employeeId)
        {
            var order = await _repository.GetOrderByIdAsync(orderId);

            if (order == null)
                return "not_found";

            await _repository.AddOrderProcessingAsync(new OrderProcessingEntity
            {
                OrderId = orderId,
                EmployeeId = employeeId,
                ActionCode = OrderActionCodes.Called,
                CreatedAt = DateTime.UtcNow
            });

            var initiator = await _repository.GetEmployeeByIdAsync(employeeId);
            await _employeesActivityService.LogAsync(employeeId, $"{initiator!.Role!.Name} {initiator.Name}({employeeId}) здійснив(ла) дзвінок по замовленню {order.OrderNumber}.", null, model.Comment);

            return "ok";
        }

        public async Task<string> TakeOrderAsync(int orderId, int employeeId)
        {
            var order = await _repository.GetOrderByIdAsync(orderId);
            if (order == null)
                return "not_found";
            if (order.TakenByEmployeeId != null)
                return "already_taken";
            order.TakenByEmployeeId = employeeId;
            await _repository.UpdateOrderAsync(order);
            order.StatusCode = OrderStatusCodes.Clarifying;
            await _notificationService.SendOrderTakenAsync();
            var initiator = await _repository.GetEmployeeByIdAsync(employeeId);
            await _employeesActivityService.LogAsync(employeeId, $"{initiator!.Role!.Name} {initiator.Name}({employeeId}) взяв(ла) замовлення {order.OrderNumber} в обробку.");
            return "ok";
        }

        public async Task<string> ReleaseOrderAsync(int orderId, int employeeId)
        {
            var order = await _repository.GetOrderByIdAsync(orderId);
            if (order == null)
                return "not_found";
            if (order.TakenByEmployeeId != employeeId)
                return "forbidden";
            order.TakenByEmployeeId = null;
            await _repository.UpdateOrderAsync(order);
            await _notificationService.SendOrderReleasedAsync();
            var initiator = await _repository.GetEmployeeByIdAsync(employeeId);
            await _employeesActivityService.LogAsync(employeeId, $"{initiator!.Role!.Name} {initiator.Name}({employeeId}) завершив(ла) обробку замовлення {order.OrderNumber}.");
            return "ok";
        }

        public async Task<string> ResetOrderAsync(int orderId, int initiatorId)
        {
            var order = await _repository.GetOrderByIdAsync(orderId);
            if (order == null)
                return "not_found";
            if (order.TakenByEmployeeId == null)
                return "not_taken";
            order.TakenByEmployeeId = null;
            await _repository.UpdateOrderAsync(order);
            order.StatusCode = OrderStatusCodes.Pending;
            await _notificationService.SendOrderResetAsync();
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) скинув(ла) активну обробку замовлення {order.OrderNumber}.");
            return "ok";
        }
    }
}