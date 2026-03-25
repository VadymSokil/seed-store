using SeedStore.Database.Entities.Store.Orders;
using SeedStore.Store.Orders.Interfaces;
using SeedStore.Store.Orders.Models;
using SeedStore.Support.General.Constants.Store;
using SeedStore.Support.General.TokenGeneration.Interfaces;
using SeedStore.Support.Store.Notifications.Interfaces;
using SeedStore.Support.Store.Payment.Interfaces;

namespace SeedStore.Store.Orders.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ITokenGenerationService _tokenGenerationService;
        private readonly IPaymentService _paymentService;
        private readonly INotificationService _notificationService;

        public OrdersService(IOrdersRepository ordersRepository, ITokenGenerationService tokenGenerationService, IPaymentService paymentService, INotificationService notificationService)
        {
            _ordersRepository = ordersRepository;
            _tokenGenerationService = tokenGenerationService;
            _paymentService = paymentService;
            _notificationService = notificationService;
        }

        public async Task<(string status, List<AccountOrdersResponseModel>? orders)> GetAccountOrdersAsync(int accountId)
        {
            var orders = await _ordersRepository.GetAccountOrdersAsync(accountId);
            return ("ok", orders);
        }

        public async Task<(string status, object? data)> AddOrderAsync(AddOrderModel model, int? accountId)
        {
            if (model.DeliveryCode == DeliveryCodes.NovaPost && string.IsNullOrEmpty(model.PostalOfficeNumber))
                return ("invalid_postal_office", null);

            if (model.DeliveryCode == DeliveryCodes.Courier &&
                (string.IsNullOrEmpty(model.City) || string.IsNullOrEmpty(model.Street) || string.IsNullOrEmpty(model.HouseNumber)))
                return ("invalid_address", null);

            var productIds = model.Items.Select(i => i.ProductId).ToList();
            var snapshots = await _ordersRepository.GetProductSnapshotsAsync(productIds);

            var orderNumber = _tokenGenerationService.GenerateOrderNumber();

            var order = new OrderEntity
            {
                OrderNumber = orderNumber,
                OrderDate = DateTime.UtcNow,
                StatusCode = OrderStatusCodes.Pending,
                Comment = model.Comment,
                DeliveryCode = model.DeliveryCode,
                PaymentCode = model.PaymentCode,
                PostalOfficeNumber = model.PostalOfficeNumber,
                TrackingNumber = null,
                TotalAmount = model.Items.Sum(i => i.Price * i.Quantity),
                AccountId = accountId,
                FirstName = model.FirstName ?? string.Empty,
                LastName = model.LastName ?? string.Empty,
                MiddleName = model.MiddleName,
                PhoneNumber = model.PhoneNumber,
                Region = model.Region,
                District = model.District,
                City = model.City,
                Settlement = model.Settlement,
                Street = model.Street,
                HouseNumber = model.HouseNumber,
                ApartmentNumber = model.ApartmentNumber,
                Items = model.Items.Select(i => new OrderItemEntity
                {
                    ProductId = i.ProductId,
                    ProductNameSnapshot = snapshots.TryGetValue(i.ProductId, out var snapshot) ? snapshot.name : string.Empty,
                    ProductImageUrlSnapshot = snapshots.TryGetValue(i.ProductId, out var snap) ? snap.imageUrl : null,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            await _ordersRepository.AddOrderAsync(order);
            await _ordersRepository.ReserveProductQuantitiesAsync(model.Items.Select(i => (i.ProductId, i.Quantity)).ToList());
            await _notificationService.SendNewOrderAsync();

            if (model.PaymentCode == PaymentCodes.Online)
            {
                var description = $"Оплата замовлення {orderNumber}";
                var (data, signature) = _paymentService.GetPaymentData(orderNumber, order.TotalAmount, description);
                return ("ok", new { data, signature });
            }

            return ("ok", null);
        }

        public async Task<string> ProcessPaymentCallbackAsync(PaymentCallbackModel model)
        {
            if (!_paymentService.ValidateCallback(model.Data, model.Signature))
                return "invalid_signature";

            var callbackData = _paymentService.DecodeCallbackData(model.Data);

            var order = await _ordersRepository.GetOrderByNumberAsync(callbackData.OrderId);
            if (order == null)
                return "not_found";

            var transaction = new OrderTransactionEntity
            {
                OrderId = order.Id,
                LiqPayPaymentId = callbackData.PaymentId,
                LiqPayOrderId = callbackData.LiqPayOrderId,
                Status = callbackData.Status,
                Amount = callbackData.Amount,
                Currency = callbackData.Currency,
                ErrCode = callbackData.ErrCode,
                ErrDescription = callbackData.ErrDescription,
                CreatedAt = DateTime.UtcNow
            };

            await _ordersRepository.AddTransactionAsync(transaction);

            if (callbackData.Status == "success")
            {
                order.StatusCode = OrderStatusCodes.Processing;
                order.PaidAt = DateTime.UtcNow;
                await _ordersRepository.UpdateOrderAsync(order);
            }

            return "ok";
        }
    }
}