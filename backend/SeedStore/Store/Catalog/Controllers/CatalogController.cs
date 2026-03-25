using Microsoft.AspNetCore.Mvc;
using SeedStore.Store.Catalog.Interfaces;

namespace SeedStore.Store.Catalog.Controllers
{
    [ApiExplorerSettings(GroupName = "store")]
    [Route("api/catalog")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _categoryService;

        public CatalogController(ICatalogService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.GetCategoriesAsync();
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchCategories([FromQuery] string value)
        {
            var result = await _categoryService.SearchCategoriesAsync(value);
            return Ok(result);
        }
    }
}
