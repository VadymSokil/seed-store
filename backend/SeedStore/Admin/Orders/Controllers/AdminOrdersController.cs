using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeedStore.Admin.Orders.Interfaces;
using SeedStore.Admin.Orders.Models;
using SeedStore.Support.Admin.Reorder.Models;
using System.Security.Claims;

namespace SeedStore.Admin.Orders.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Policy = "AdminPolicy")]
    [Route("api/admin/orders")]
    [ApiController]
    public class AdminOrdersController : ControllerBase
    {
        private readonly IAdminOrdersService _adminOrdersService;

        public AdminOrdersController(IAdminOrdersService adminOrdersService)
        {
            _adminOrdersService = adminOrdersService;
        }

        [HttpGet]
        [Authorize(Policy = "Permission:orders.manage")]
        public async Task<IActionResult> GetOrders([FromQuery] string? statusCode)
        {
            var result = await _adminOrdersService.GetOrdersAsync(statusCode);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Permission:orders.manage")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var result = await _adminOrdersService.GetOrderAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Permission:orders.manage")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.UpdateOrderAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{id}/status")]
        [Authorize(Policy = "Permission:orders.manage")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusCodeModel model)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.UpdateOrderStatusAsync(id, model.StatusCode, employeeId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPost("{id}/call")]
        [Authorize(Policy = "Permission:orders.manage")]
        public async Task<IActionResult> LogCall(int id, [FromBody] LogCallModel model)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.LogCallAsync(id, model, employeeId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPost("{id}/pack")]
        [Authorize(Policy = "Permission:orders.tracking")]
        public async Task<IActionResult> PackOrder(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.PackOrderAsync(id, employeeId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "already_packed" => Conflict("already_packed"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{id}/tracking")]
        [Authorize(Policy = "Permission:orders.tracking")]
        public async Task<IActionResult> UpdateTrackingNumber(int id, [FromBody] UpdateTrackingNumberModel model)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.UpdateTrackingNumberAsync(id, model.TrackingNumber, employeeId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "not_packed" => BadRequest("not_packed"),
                _ => StatusCode(500)
            };
        }

        [HttpGet("actions")]
        [Authorize(Policy = "Permission:orders.manage")]
        public async Task<IActionResult> GetOrderActions()
        {
            var result = await _adminOrdersService.GetOrderActionsAsync();
            return Ok(result);
        }

        [HttpPost("actions")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> AddOrderAction([FromBody] AddOrderActionModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.AddOrderActionAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("actions/{id}")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> UpdateOrderAction(int id, [FromBody] UpdateOrderActionModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.UpdateOrderActionAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("actions/{id}")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> DeleteOrderAction(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.DeleteOrderActionAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpGet("statuses")]
        [Authorize(Policy = "Permission:orders.manage")]
        public async Task<IActionResult> GetOrderStatuses()
        {
            var result = await _adminOrdersService.GetOrderStatusesAsync();
            return Ok(result);
        }

        [HttpPost("statuses")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> AddOrderStatus([FromBody] AddOrderStatusModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.AddOrderStatusAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "code_taken" => Conflict("code_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("statuses/{id}")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.UpdateOrderStatusAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "code_taken" => Conflict("code_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("statuses/{id}")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> DeleteOrderStatus(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.DeleteOrderStatusAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("reorder/order-statuses")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> ReorderOrderStatuses([FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _adminOrdersService.ReorderOrderStatusesAsync(items, initiatorId);
            return Ok();
        }

        [HttpPut("reorder/order-actions")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> ReorderOrderActions([FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _adminOrdersService.ReorderOrderActionsAsync(items, initiatorId);
            return Ok();
        }

        [HttpPost("{id}/take")]
        [Authorize(Policy = "Permission:orders.manage")]
        public async Task<IActionResult> TakeOrder(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.TakeOrderAsync(id, employeeId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "already_taken" => Conflict("already_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpPost("{id}/release")]
        [Authorize(Policy = "Permission:orders.manage")]
        public async Task<IActionResult> ReleaseOrder(int id)
        {
            var employeeId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.ReleaseOrderAsync(id, employeeId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "forbidden" => Forbid(),
                _ => StatusCode(500)
            };
        }

        [HttpPost("{id}/reset")]
        [Authorize(Policy = "Permission:orders.manage")]
        public async Task<IActionResult> ResetOrder(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminOrdersService.ResetOrderAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "not_taken" => BadRequest("not_taken"),
                _ => StatusCode(500)
            };
        }
    }
}