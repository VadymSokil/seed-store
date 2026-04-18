using Microsoft.AspNetCore.Mvc;
using SeedStore.Store.StoreInfo.Interfaces;

namespace SeedStore.Store.StoreInfo.Controllers
{
    [ApiExplorerSettings(GroupName = "store")]
    [Route("api/store-info")]
    [ApiController]
    public class StoreInfoController : ControllerBase
    {
        private readonly IStoreInfoService _storeInfoService;

        public StoreInfoController(IStoreInfoService storeInfoService)
        {
            _storeInfoService = storeInfoService;
        }

        [HttpGet("delivery-variants")]
        public async Task<IActionResult> GetDeliveryVariants()
        {
            var result = await _storeInfoService.GetDeliveryVariantsAsync();
            return Ok(result);
        }

        [HttpGet("payment-variants")]
        public async Task<IActionResult> GetPaymentVariants()
        {
            var result = await _storeInfoService.GetPaymentVariantsAsync();
            return Ok(result);
        }

        [HttpGet("about")]
        public async Task<IActionResult> GetAboutPage()
        {
            var result = await _storeInfoService.GetAboutPageAsync();
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("contacts")]
        public async Task<IActionResult> GetContacts()
        {
            var result = await _storeInfoService.GetContactsAsync();
            if (result == null) return NotFound();
            return Ok(result);
        }
    }
}
