using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeedStore.Store.Orders.Interfaces;
using SeedStore.Store.Orders.Models;
using System.Security.Claims;

namespace SeedStore.Store.Orders.Controllers
{
    [ApiExplorerSettings(GroupName = "store")]
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [Authorize(Policy = "StorePolicy")]
        [HttpGet]
        public async Task<IActionResult> GetAccountOrders()
        {
            var accountId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var (status, orders) = await _ordersService.GetAccountOrdersAsync(accountId);

            return status switch
            {
                "ok" => Ok(orders),
                _ => StatusCode(500)
            };
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] AddOrderModel model)
        {
            int? accountId = User.FindFirst(ClaimTypes.NameIdentifier) is { } claim
                ? int.Parse(claim.Value)
                : null;

            var (status, data) = await _ordersService.AddOrderAsync(model, accountId);

            return status switch
            {
                "ok" => Ok(data),
                "invalid_postal_office" => BadRequest(new { message = "Вкажіть номер відділення" }),
                "invalid_address" => BadRequest(new { message = "Вкажіть адресу доставки" }),
                _ => StatusCode(500)
            };
        }

        [HttpPost("liqpay-callback")]
        public async Task<IActionResult> PaymentCallback([FromForm] PaymentCallbackModel model)
        {
            var status = await _ordersService.ProcessPaymentCallbackAsync(model);

            return status switch
            {
                "ok" => Ok(),
                "invalid_signature" => Ok(),
                "not_found" => Ok(),
                _ => Ok()
            };
        }
    }
}