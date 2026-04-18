using Microsoft.EntityFrameworkCore;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Store.Orders;
using SeedStore.Store.Orders.Interfaces;
using SeedStore.Store.Orders.Models;

namespace SeedStore.Store.Orders.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly AppDbContext _context;

        public OrdersRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AccountOrdersResponseModel> GetAccountOrdersAsync(int accountId, int page, int pageSize)
        {
            var query = _context.Orders
                .Where(o => o.AccountId == accountId)
                .OrderByDescending(o => o.OrderDate);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(o => new AccountOrderModel
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate,
                    StatusName = o.Status!.Name,
                    Comment = o.Comment,
                    DeliveryCode = o.DeliveryCode,
                    PaymentCode = o.PaymentCode,
                    PostalOfficeNumber = o.PostalOfficeNumber,
                    TrackingNumber = o.TrackingNumber,
                    TotalAmount = o.TotalAmount,
                    FirstName = o.FirstName,
                    LastName = o.LastName,
                    MiddleName = o.MiddleName,
                    PhoneNumber = o.PhoneNumber,
                    Region = o.Region,
                    District = o.District,
                    City = o.City,
                    Settlement = o.Settlement,
                    Street = o.Street,
                    HouseNumber = o.HouseNumber,
                    ApartmentNumber = o.ApartmentNumber,
                    PaidAt = o.PaidAt,
                    Items = o.Items.Select(i => new OrderItemsModel
                    {
                        ProductId = i.ProductId,
                        ProductNameSnapshot = i.ProductNameSnapshot,
                        ProductImageUrlSnapshot = i.ProductImageUrlSnapshot,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList(),
                    Transactions = o.Transactions.Select(t => new OrderTransactionsModel
                    {
                        Amount = t.Amount,
                        Currency = t.Currency,
                        CreatedAt = t.CreatedAt,
                        IsSuccess = t.Status == "success"
                    }).ToList()
                })
                .ToListAsync();

            return new AccountOrdersResponseModel
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task AddOrderAsync(OrderEntity order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Dictionary<int, (string name, string? imageUrl)>> GetProductSnapshotsAsync(List<int> productIds)
        {
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .Select(p => new { p.Id, p.Name })
                .ToListAsync();

            var images = await _context.ProductImages
                .Where(i => productIds.Contains(i.ProductId))
                .OrderBy(i => i.ViewOrder)
                .GroupBy(i => i.ProductId)
                .Select(g => new { ProductId = g.Key, g.First().Url })
                .ToListAsync();

            return products.ToDictionary(
                p => p.Id,
                p => (p.Name, images.FirstOrDefault(i => i.ProductId == p.Id)?.Url)
            );
        }

        public async Task<OrderEntity?> GetOrderByNumberAsync(string orderNumber)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task AddTransactionAsync(OrderTransactionEntity transaction)
        {
            await _context.OrderTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateOrderAsync(OrderEntity order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task ReserveProductQuantitiesAsync(List<(int productId, int quantity)> items)
        {
            foreach (var (productId, quantity) in items)
            {
                await _context.Products
                    .Where(p => p.Id == productId)
                    .ExecuteUpdateAsync(s => s.SetProperty(p => p.Quantity, p => p.Quantity - quantity));
            }
        }
    }
}