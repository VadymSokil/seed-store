using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SeedStore.Admin.Products.Interfaces;
using SeedStore.Admin.Products.Models;
using SeedStore.Support.Admin.Reorder.Models;
using System.Security.Claims;

namespace SeedStore.Admin.Products.Controllers
{
    [ApiExplorerSettings(GroupName = "admin")]
    [Authorize(Policy = "AdminPolicy")]
    [Route("api/admin/products")]
    [ApiController]
    public class AdminProductsController : ControllerBase
    {
        private readonly IAdminProductsService _adminProductService;

        public AdminProductsController(IAdminProductsService adminProductService)
        {
            _adminProductService = adminProductService;
        }

        [HttpGet]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> GetProducts([FromQuery] int? categoryId, [FromQuery] bool? isActive)
        {
            var result = await _adminProductService.GetProductsAsync(categoryId, isActive);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _adminProductService.GetProductAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.AddProductAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "slug_taken" => Conflict("slug_taken"),
                "article_taken" => Conflict("article_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.UpdateProductAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                "slug_taken" => Conflict("slug_taken"),
                "article_taken" => Conflict("article_taken"),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.DeleteProductAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpGet("{id}/images")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> GetProductImages(int id)
        {
            var result = await _adminProductService.GetProductImagesAsync(id);
            return Ok(result);
        }

        [HttpPost("{id}/images")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> AddProductImage(int id, [FromBody] AddProductImageModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.AddProductImageAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{id}/images/{imageId}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> UpdateProductImage(int id, int imageId, [FromBody] UpdateProductImageModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.UpdateProductImageAsync(imageId, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("{id}/images/{imageId}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> DeleteProductImage(int id, int imageId)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.DeleteProductImageAsync(imageId, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpGet("{id}/features")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> GetProductFeatures(int id)
        {
            var result = await _adminProductService.GetProductFeaturesAsync(id);
            return Ok(result);
        }

        [HttpPost("{id}/features")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> AddProductFeature(int id, [FromBody] AddProductFeatureModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.AddProductFeatureAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("{id}/features/{featureId}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> UpdateProductFeature(int id, int featureId, [FromBody] UpdateProductFeatureModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.UpdateProductFeatureAsync(featureId, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("{id}/features/{featureId}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> DeleteProductFeature(int id, int featureId)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.DeleteProductFeatureAsync(featureId, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpGet("features")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> GetFeatures([FromQuery] int categoryId)
        {
            var result = await _adminProductService.GetFeaturesAsync(categoryId);
            return Ok(result);
        }

        [HttpPost("features")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> AddFeature([FromBody] AddFeatureModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.AddFeatureAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("features/{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> UpdateFeature(int id, [FromBody] UpdateFeatureModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.UpdateFeatureAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("features/{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.DeleteFeatureAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpGet("feature-headers")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> GetFeatureHeaders([FromQuery] int categoryId)
        {
            var result = await _adminProductService.GetFeatureHeadersAsync(categoryId);
            return Ok(result);
        }

        [HttpPost("feature-headers")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> AddFeatureHeader([FromBody] AddFeatureHeaderModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.AddFeatureHeaderAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("feature-headers/{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> UpdateFeatureHeader(int id, [FromBody] UpdateFeatureHeaderModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.UpdateFeatureHeaderAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("feature-headers/{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> DeleteFeatureHeader(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.DeleteFeatureHeaderAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpGet("filter-values")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> GetFilterValues([FromQuery] int categoryId)
        {
            var result = await _adminProductService.GetFilterValuesAsync(categoryId);
            return Ok(result);
        }

        [HttpPost("filter-values")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> AddFilterValue([FromBody] AddFilterValueModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.AddFilterValueAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("filter-values/{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> UpdateFilterValue(int id, [FromBody] UpdateFilterValueModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.UpdateFilterValueAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("filter-values/{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> DeleteFilterValue(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.DeleteFilterValueAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpGet("discount-groups")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> GetDiscountGroups()
        {
            var result = await _adminProductService.GetDiscountGroupsAsync();
            return Ok(result);
        }

        [HttpPost("discount-groups")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> AddDiscountGroup([FromBody] AddDiscountGroupModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.AddDiscountGroupAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("discount-groups/{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> UpdateDiscountGroup(int id, [FromBody] UpdateDiscountGroupModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.UpdateDiscountGroupAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("discount-groups/{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> DeleteDiscountGroup(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.DeleteDiscountGroupAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpGet("discounts")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> GetDiscounts([FromQuery] int groupId)
        {
            var result = await _adminProductService.GetDiscountsAsync(groupId);
            return Ok(result);
        }

        [HttpPost("discounts")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> AddDiscount([FromBody] AddDiscountModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.AddDiscountAsync(model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "group_not_found" => NotFound("group_not_found"),
                "discount_exists" => Conflict("discount_exists"),
                _ => StatusCode(500)
            };
        }

        [HttpPut("discounts/{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> UpdateDiscount(int id, [FromBody] UpdateDiscountModel model)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.UpdateDiscountAsync(id, model, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("discounts/{id}")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _adminProductService.DeleteDiscountAsync(id, initiatorId);

            return result switch
            {
                "ok" => Ok(),
                "not_found" => NotFound(),
                _ => StatusCode(500)
            };
        }

        [HttpPut("reorder/product-features")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> ReorderProductFeatures([FromQuery] int productId, [FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _adminProductService.ReorderProductFeaturesAsync(productId, items, initiatorId);
            return Ok();
        }

        [HttpPut("reorder/product-images")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> ReorderProductImages([FromQuery] int productId, [FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _adminProductService.ReorderProductImagesAsync(productId, items, initiatorId);
            return Ok();
        }

        [HttpPut("reorder/filter-values")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> ReorderFilterValues([FromQuery] int categoryId, [FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _adminProductService.ReorderFilterValuesAsync(categoryId, items, initiatorId);
            return Ok();
        }

        [HttpPut("reorder/feature-headers")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> ReorderFeatureHeaders([FromQuery] int categoryId, [FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _adminProductService.ReorderFeatureHeadersAsync(categoryId, items, initiatorId);
            return Ok();
        }

        [HttpPut("reorder/features")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> ReorderFeatures([FromQuery] int categoryId, [FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _adminProductService.ReorderFeaturesAsync(categoryId, items, initiatorId);
            return Ok();
        }

        [HttpPut("reorder/discount-groups")]
        [Authorize(Policy = "Permission:products.manage")]
        public async Task<IActionResult> ReorderDiscountGroups([FromBody] List<ReorderItemModel> items)
        {
            var initiatorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _adminProductService.ReorderDiscountGroupsAsync(items, initiatorId);
            return Ok();
        }
    }
}