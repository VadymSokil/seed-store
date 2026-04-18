using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeedStore.Admin.StoreInfo.Interfaces;
using SeedStore.Admin.StoreInfo.Models;
using SeedStore.Support.Admin.Reorder.Models;
using System.Security.Claims;

namespace SeedStore.Admin.StoreInfo.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Policy = "AdminPolicy")]
    [Route("api/admin/store-info")]
    [ApiController]
    public class AdminStoreInfoController : ControllerBase
    {
        private readonly IAdminStoreInfoService _adminStoreInfoService;

        public AdminStoreInfoController(IAdminStoreInfoService adminStoreInfoService)
        {
            _adminStoreInfoService = adminStoreInfoService;
        }

        [HttpGet("delivery-variants")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> GetDeliveryVariants()
        {
            var result = await _adminStoreInfoService.GetDeliveryVariantsAsync();
            return Ok(result);
        }

        [HttpPost("delivery-variants")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> AddDeliveryVariant([FromBody] AddDeliveryVariantModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminStoreInfoService.AddDeliveryVariantAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "code_taken" => Conflict("code_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("delivery-variants/{id}")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> UpdateDeliveryVariant(int id, [FromBody] UpdateDeliveryVariantModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminStoreInfoService.UpdateDeliveryVariantAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "code_taken" => Conflict("code_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("delivery-variants/{id}")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> DeleteDeliveryVariant(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminStoreInfoService.DeleteDeliveryVariantAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpGet("payment-variants")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> GetPaymentVariants()
        {
            var result = await _adminStoreInfoService.GetPaymentVariantsAsync();
            return Ok(result);
        }

        [HttpPost("payment-variants")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> AddPaymentVariant([FromBody] AddPaymentVariantModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminStoreInfoService.AddPaymentVariantAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "code_taken" => Conflict("code_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("payment-variants/{id}")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> UpdatePaymentVariant(int id, [FromBody] UpdatePaymentVariantModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminStoreInfoService.UpdatePaymentVariantAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "code_taken" => Conflict("code_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("payment-variants/{id}")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> DeletePaymentVariant(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminStoreInfoService.DeletePaymentVariantAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("reorder/delivery-variants")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> ReorderDeliveryVariants([FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _adminStoreInfoService.ReorderDeliveryVariantsAsync(items, initiatorId);
            return Ok();
        }

        [HttpPut("reorder/payment-variants")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> ReorderPaymentVariants([FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _adminStoreInfoService.ReorderPaymentVariantsAsync(items, initiatorId);
            return Ok();
        }

        [HttpGet("about")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> GetAboutPage()
        {
            var result = await _adminStoreInfoService.GetAboutPageAsync();
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut("about")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> UpdateAboutPage([FromBody] UpdatePageContentModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminStoreInfoService.UpdateAboutPageAsync(model, initiatorId);
            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpGet("contacts")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> GetContacts()
        {
            var result = await _adminStoreInfoService.GetContactsAsync();
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut("contacts")]
        [Authorize(Policy = "Permission:store.manage")]
        public async Task<IActionResult> UpdateContacts([FromBody] UpdatePageContentModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminStoreInfoService.UpdateContactsAsync(model, initiatorId);
            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }
    }
}