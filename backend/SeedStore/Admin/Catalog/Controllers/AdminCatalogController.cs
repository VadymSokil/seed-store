using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeedStore.Admin.Catalog.Interfaces;
using SeedStore.Admin.Catalog.Models;
using SeedStore.Support.Admin.Reorder.Models;
using System.Security.Claims;

namespace SeedStore.Admin.Catalog.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Policy = "AdminPolicy")]
    [Route("api/admin/catalog")]
    [ApiController]
    public class AdminCatalogController : ControllerBase
    {
        private readonly IAdminCatalogService _adminCategoryService;

        public AdminCatalogController(IAdminCatalogService adminCategoryService)
        {
            _adminCategoryService = adminCategoryService;
        }

        [HttpGet]
        [Authorize(Policy = "Permission:catalog.manage")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _adminCategoryService.GetCategoriesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Permission:catalog.manage")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await _adminCategoryService.GetCategoryAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Permission:catalog.manage")]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminCategoryService.AddCategoryAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "slug_taken" => Conflict("slug_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Permission:catalog.manage")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminCategoryService.UpdateCategoryAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "slug_taken" => Conflict("slug_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Permission:catalog.manage")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminCategoryService.DeleteCategoryAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("reorder")]
        [Authorize(Policy = "Permission:catalog.manage")]
        public async Task<IActionResult> ReorderCategories([FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _adminCategoryService.ReorderCategoriesAsync(items, initiatorId);
            return Ok();
        }
    }
}