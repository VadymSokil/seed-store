using Microsoft.AspNetCore.Mvc;
using seed_store_api.Store.Modules.Catalog.Interfaces;

namespace seed_store_api.Store.Modules.Catalog.Controllers
{
    [ApiExplorerSettings(GroupName = "store")]
    [Route("categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
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
