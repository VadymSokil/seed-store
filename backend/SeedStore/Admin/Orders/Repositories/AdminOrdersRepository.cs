using Microsoft.EntityFrameworkCore;
using SeedStore.Admin.Orders.Interfaces;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Admin.Orders;
using SeedStore.Database.Entities.Store.Orders;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Orders.Repositories
{
    public class AdminOrdersRepository : IAdminOrdersRepository
    {
        private readonly AppDbContext _context;

        public AdminOrdersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderEntity>> GetOrdersAsync(string? statusCode)
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(statusCode))
                query = query.Where(o => o.StatusCode == statusCode);

            return await query.OrderByDescending(o => o.OrderDate).ToListAsync();
        }

        public async Task<OrderEntity?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<OrderItemEntity?> GetOrderItemByIdAsync(int itemId)
        {
            return await _context.OrderItems
                .FirstOrDefaultAsync(i => i.Id == itemId);
        }

        public async Task UpdateOrderAsync(OrderEntity order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderItemAsync(OrderItemEntity item)
        {
            _context.OrderItems.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task AddOrderProcessingAsync(OrderProcessingEntity processing)
        {
            await _context.OrderProcessings.AddAsync(processing);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderActionEntity>> GetOrderActionsAsync()
        {
            return await _context.OrderActions.OrderBy(a => a.ViewOrder).ToListAsync();
        }

        public async Task<OrderActionEntity?> GetOrderActionByIdAsync(int actionId)
        {
            return await _context.OrderActions.FirstOrDefaultAsync(a => a.Id == actionId);
        }

        public async Task AddOrderActionAsync(OrderActionEntity action)
        {
            action.ViewOrder = await GetNextOrderActionViewOrderAsync();
            await _context.OrderActions.AddAsync(action);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderActionAsync(OrderActionEntity action)
        {
            _context.OrderActions.Update(action);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderActionAsync(OrderActionEntity action)
        {
            _context.OrderActions.Remove(action);
            await _context.SaveChangesAsync();
        }

        public async Task<List<OrderStatusEntity>> GetOrderStatusesAsync()
        {
            return await _context.OrderStatuses.OrderBy(s => s.ViewOrder).ToListAsync();
        }

        public async Task<OrderStatusEntity?> GetOrderStatusByIdAsync(int statusId)
        {
            return await _context.OrderStatuses.FirstOrDefaultAsync(s => s.Id == statusId);
        }

        public async Task<bool> OrderStatusCodeExistsAsync(string code)
        {
            return await _context.OrderStatuses.AnyAsync(s => s.Code == code);
        }

        public async Task AddOrderStatusAsync(OrderStatusEntity status)
        {
            status.ViewOrder = await GetNextOrderStatusViewOrderAsync();
            await _context.OrderStatuses.AddAsync(status);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderStatusAsync(OrderStatusEntity status)
        {
            _context.OrderStatuses.Update(status);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderStatusAsync(OrderStatusEntity status)
        {
            _context.OrderStatuses.Remove(status);
            await _context.SaveChangesAsync();
        }
        public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task<int> GetNextOrderStatusViewOrderAsync()
        {
            return (await _context.OrderStatuses.MaxAsync(c => c.ViewOrder) ?? 0) + 1;
        }

        public async Task<int> GetNextOrderActionViewOrderAsync()
        {
            return (await _context.OrderActions.MaxAsync(c => c.ViewOrder) ?? 0) + 1;
        }

        public async Task ReorderOrderStatusesAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.OrderStatuses
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task ReorderOrderActionsAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.OrderActions
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task<bool> OrderProcessingExistsAsync(int orderId, string actionCode)
        {
            return await _context.OrderProcessings
                .AnyAsync(p => p.OrderId == orderId && p.ActionCode == actionCode);
        }

        public async Task<List<OrderItemEntity>> GetOrderItemsAsync(int orderId)
        {
            return await _context.OrderItems
                .Where(i => i.OrderId == orderId)
                .ToListAsync();
        }

        public async Task RestoreProductQuantitiesAsync(List<(int productId, int quantity)> items)
        {
            foreach (var (productId, quantity) in items)
            {
                await _context.Products
                    .Where(p => p.Id == productId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.Quantity, p => p.Quantity + quantity));
            }
        }

        public async Task<List<ReorderItemModel>> GetOrderStatusesOrderAsync()
        {
            return await _context.OrderStatuses
                .OrderBy(s => s.ViewOrder)
                .Select(s => new ReorderItemModel { Id = s.Id, ViewOrder = s.ViewOrder ?? 0 })
                .ToListAsync();
        }

        public async Task<List<ReorderItemModel>> GetOrderActionsOrderAsync()
        {
            return await _context.OrderActions
                .OrderBy(a => a.ViewOrder)
                .Select(a => new ReorderItemModel { Id = a.Id, ViewOrder = a.ViewOrder ?? 0 })
                .ToListAsync();
        }
    }
}