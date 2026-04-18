using Microsoft.AspNetCore.Mvc;
using SeedStore.Store.Products.Interfaces;
using SeedStore.Store.Products.Models;

namespace SeedStore.Store.Products.Controllers
{
    [ApiExplorerSettings(GroupName = "store")]
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string value)
        {
            var result = await _productService.SearchProductsAsync(value);
            return Ok(result);
        }

        [HttpGet("top")]
        public async Task<IActionResult> GetProductsTop()
        {
            var result = await _productService.GetProductsTopAsync();
            return Ok(result);
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetCategoryFiltersAsync([FromQuery] int categoryId)
        {
            var result = await _productService.GetCategoryFiltersAsync(categoryId);
            return Ok(result);
        }

        [HttpPost("list")]
        public async Task<IActionResult> GetProductsList([FromBody] ProductListRequestModel request)
        {
            var result = await _productService.GetProductsListAsync(request);
            return Ok(result);
        }

        [HttpGet("{idOrSlug}")]
        public async Task<IActionResult> GetProductDetailsAsync([FromRoute] string idOrSlug)
        {
            var result = await _productService.GetProductDetailsAsync(idOrSlug);
            return Ok(result);
        }

        [HttpGet("discounts")]
        public async Task<IActionResult> GetActiveDiscountGroups()
        {
            var result = await _productService.GetActiveDiscountGroupsAsync();
            return Ok(result);
        }
    }
}
