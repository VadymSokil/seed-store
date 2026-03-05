using Microsoft.AspNetCore.Mvc;
using seed_store_api.Store.Modules.StoreInfo.Interfaces;

namespace seed_store_api.Store.Modules.StoreInfo.Controllers
{
    [ApiExplorerSettings(GroupName = "store")]
    [Route("store-info")]
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
    }
}
