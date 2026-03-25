using Microsoft.Extensions.Caching.Memory;
using SeedStore.Admin.Products.Interfaces;
using SeedStore.Admin.Products.Models;
using SeedStore.Database.Entities.Store.Products;
using SeedStore.Support.Admin.EmployeesActivity.Interfaces;
using SeedStore.Support.Admin.Reorder.Models;
using SeedStore.Support.General.Constants.CacheKeys;
using System.Text.Json;

namespace SeedStore.Admin.Products.Services
{
    public class AdminProductsService : IAdminProductsService
    {
        private readonly IAdminProductsRepository _repository;
        private readonly IEmployeesActivityService _employeesActivityService;
        private readonly IMemoryCache _cache;

        public AdminProductsService(IAdminProductsRepository repository, IEmployeesActivityService employeesActivityService, IMemoryCache cache)
        {
            _repository = repository;
            _employeesActivityService = employeesActivityService;
            _cache = cache;
        }

        public async Task<List<ResponseProductsModel>> GetProductsAsync(int? categoryId, bool? isActive)
        {
            var products = await _repository.GetProductsAsync(categoryId, isActive);

            return products.Select(p => new ResponseProductsModel
            {
                Id = p.Id,
                CategoryId = p.CategoryId,
                Article = p.Article,
                Name = p.Name,
                Slug = p.Slug,
                Price = p.Price,
                Quantity = p.Quantity,
                IsActive = p.IsActive,
                CreatedDate = p.CreatedDate
            }).ToList();
        }

        public async Task<ResponseProductModel?> GetProductAsync(int id)
        {
            var product = await _repository.GetProductByIdAsync(id);

            if (product == null)
                return null;

            var images = await _repository.GetProductImagesAsync(id);
            var features = await _repository.GetProductFeaturesAsync(id);

            return new ResponseProductModel
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Article = product.Article,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                Rating = product.Rating,
                ReviewCount = product.ReviewCount,
                IsActive = product.IsActive,
                CreatedDate = product.CreatedDate,
                Images = images.Select(i => new ProductImageModel
                {
                    Id = i.Id,
                    Url = i.Url,
                    ViewOrder = i.ViewOrder
                }).ToList(),
                Features = features.Select(f => new ProductFeatureModel
                {
                    Id = f.Id,
                    FeatureId = f.FeatureId,
                    FeatureName = f.Feature!.Name,
                    HeaderId = f.HeaderId,
                    HeaderName = f.Header?.Name,
                    Value = f.Value,
                    IsActive = f.IsActive,
                    ViewOrder = f.ViewOrder
                }).ToList()
            };
        }

        public async Task<string> AddProductAsync(AddProductModel model, int initiatorId)
        {
            var slugExists = await _repository.SlugExistsAsync(model.Slug);
            if (slugExists)
                return "slug_taken";

            var articleExists = await _repository.ArticleExistsAsync(model.Article);
            if (articleExists)
                return "article_taken";

            await _repository.AddProductAsync(new ProductEntity
            {
                CategoryId = model.CategoryId,
                Article = model.Article,
                Name = model.Name,
                Slug = model.Slug,
                Description = model.Description,
                Price = model.Price,
                Quantity = model.Quantity,
                IsActive = model.IsActive,
                Rating = 0,
                ReviewCount = 0,
                CreatedDate = DateTime.UtcNow
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) товар {model.Name}.");

            _cache.Remove(CacheKeys.Top);
            return "ok";
        }

        public async Task<string> UpdateProductAsync(int id, UpdateProductModel model, int initiatorId)
        {
            var product = await _repository.GetProductByIdAsync(id);

            if (product == null)
                return "not_found";

            var slugExists = await _repository.SlugExistsAsync(model.Slug);
            if (slugExists && product.Slug != model.Slug)
                return "slug_taken";

            var articleExists = await _repository.ArticleExistsAsync(model.Article);
            if (articleExists && product.Article != model.Article)
                return "article_taken";

            var before = JsonSerializer.Serialize(new { product.CategoryId, product.Article, product.Name, product.Slug, product.Description, product.Price, product.Quantity, product.IsActive });

            product.CategoryId = model.CategoryId;
            product.Article = model.Article;
            product.Name = model.Name;
            product.Slug = model.Slug;
            product.Description = model.Description;
            product.Price = model.Price;
            product.Quantity = model.Quantity;
            product.IsActive = model.IsActive;
            await _repository.UpdateProductAsync(product);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = JsonSerializer.Serialize(new { model.CategoryId, model.Article, model.Name, model.Slug, model.Description, model.Price, model.Quantity, model.IsActive });
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) товар {product.Name}.", before, after);

            _cache.Remove(CacheKeys.Top);
            return "ok";
        }

        public async Task<string> DeleteProductAsync(int id, int initiatorId)
        {
            var product = await _repository.GetProductByIdAsync(id);

            if (product == null)
                return "not_found";

            await _repository.DeleteProductAsync(product);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) товар {product.Name}.");

            _cache.Remove(CacheKeys.Top);
            return "ok";
        }

        public async Task<string> DeleteProductImageAsync(int imageId, int initiatorId)
        {
            var image = await _repository.GetProductImageByIdAsync(imageId);

            if (image == null)
                return "not_found";

            await _repository.DeleteProductImageAsync(image);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var product = await _repository.GetProductByIdAsync(image.ProductId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) зображення товару {product!.Name}.");

            _cache.Remove(CacheKeys.Top);
            return "ok";
        }

        public async Task<List<ResponseProductImageModel>> GetProductImagesAsync(int productId)
        {
            var images = await _repository.GetProductImagesAsync(productId);

            return images.Select(i => new ResponseProductImageModel
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Url = i.Url,
                ViewOrder = i.ViewOrder
            }).ToList();
        }

        public async Task<string> AddProductImageAsync(int productId, AddProductImageModel model, int initiatorId)
        {
            var product = await _repository.GetProductByIdAsync(productId);

            if (product == null)
                return "not_found";

            await _repository.AddProductImageAsync(new ProductImageEntity
            {
                ProductId = productId,
                Url = model.Url,
                ViewOrder = model.ViewOrder
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) зображення до товару {product.Name}.");

            _cache.Remove(CacheKeys.Top);
            return "ok";
        }

        public async Task<string> UpdateProductImageAsync(int imageId, UpdateProductImageModel model, int initiatorId)
        {
            var image = await _repository.GetProductImageByIdAsync(imageId);

            if (image == null)
                return "not_found";

            var before = image.Url;

            image.Url = model.Url;
            image.ViewOrder = model.ViewOrder;
            await _repository.UpdateProductImageAsync(image);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var product = await _repository.GetProductByIdAsync(image.ProductId);
            var after = model.Url;
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) зображення до товару {product!.Name}.", before, after);

            _cache.Remove(CacheKeys.Top);
            return "ok";
        }

        public async Task<List<ResponseProductFeatureModel>> GetProductFeaturesAsync(int productId)
        {
            var features = await _repository.GetProductFeaturesAsync(productId);

            return features.Select(f => new ResponseProductFeatureModel
            {
                Id = f.Id,
                FeatureId = f.FeatureId,
                FeatureName = f.Feature!.Name,
                HeaderId = f.HeaderId,
                HeaderName = f.Header?.Name,
                Value = f.Value,
                IsActive = f.IsActive,
                ViewOrder = f.ViewOrder
            }).ToList();
        }

        public async Task<string> AddProductFeatureAsync(int productId, AddProductFeatureModel model, int initiatorId)
        {
            var product = await _repository.GetProductByIdAsync(productId);

            if (product == null)
                return "not_found";

            await _repository.AddProductFeatureAsync(new ProductFeatureEntity
            {
                ProductId = productId,
                FeatureId = model.FeatureId,
                HeaderId = model.HeaderId,
                Value = model.Value,
                ValueSlug = model.ValueSlug,
                IsActive = model.IsActive,
                ViewOrder = model.ViewOrder
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) характеристику {model.Value} до товару {product.Name}.");

            return "ok";
        }

        public async Task<string> UpdateProductFeatureAsync(int featureId, UpdateProductFeatureModel model, int initiatorId)
        {
            var feature = await _repository.GetProductFeatureByIdAsync(featureId);

            if (feature == null)
                return "not_found";

            var before = JsonSerializer.Serialize(new { feature.FeatureId, feature.Value, feature.IsActive, feature.ViewOrder });

            feature.FeatureId = model.FeatureId;
            feature.HeaderId = model.HeaderId;
            feature.Value = model.Value;
            feature.ValueSlug = model.ValueSlug;
            feature.IsActive = model.IsActive;
            feature.ViewOrder = model.ViewOrder;
            await _repository.UpdateProductFeatureAsync(feature);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = JsonSerializer.Serialize(new { model.FeatureId, model.Value, model.IsActive, model.ViewOrder });
            var product = await _repository.GetProductByIdAsync(feature.ProductId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) характеристику {feature.Value} до товару {product!.Name}.", before, after);

            return "ok";
        }

        public async Task<string> DeleteProductFeatureAsync(int featureId, int initiatorId)
        {
            var feature = await _repository.GetProductFeatureByIdAsync(featureId);

            if (feature == null)
                return "not_found";

            await _repository.DeleteProductFeatureAsync(feature);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var product = await _repository.GetProductByIdAsync(feature.ProductId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) характеристику {feature.Value} до товару {product!.Name}.");

            return "ok";
        }

        public async Task<List<ResponseFeatureModel>> GetFeaturesAsync(int categoryId)
        {
            var features = await _repository.GetFeaturesAsync(categoryId);

            return features.Select(f => new ResponseFeatureModel
            {
                Id = f.Id,
                CategoryId = f.CategoryId,
                Name = f.Name,
                Slug = f.Slug,
                IsActive = f.IsActive,
                ViewOrder = f.ViewOrder
            }).ToList();
        }

        public async Task<string> AddFeatureAsync(AddFeatureModel model, int initiatorId)
        {
            await _repository.AddFeatureAsync(new FeatureEntity
            {
                CategoryId = model.CategoryId,
                Name = model.Name,
                Slug = model.Slug,
                ViewOrder = model.ViewOrder
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var category = await _repository.GetCategoryByIdAsync(model.CategoryId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) характеристику {model.Name} до категорії {category!.Name}.");

            return "ok";
        }

        public async Task<string> UpdateFeatureAsync(int id, UpdateFeatureModel model, int initiatorId)
        {
            var feature = await _repository.GetFeatureByIdAsync(id);

            if (feature == null)
                return "not_found";

            var before = JsonSerializer.Serialize(new { feature.CategoryId, feature.Name, feature.Slug, feature.IsActive, feature.ViewOrder });

            feature.CategoryId = model.CategoryId;
            feature.Name = model.Name;
            feature.Slug = model.Slug;
            feature.IsActive = model.IsActive;
            feature.ViewOrder = model.ViewOrder;
            await _repository.UpdateFeatureAsync(feature);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var category = await _repository.GetCategoryByIdAsync(model.CategoryId);
            var after = JsonSerializer.Serialize(new { model.CategoryId, model.Name, model.Slug, model.IsActive, model.ViewOrder });
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) характеристику {feature.Name} до категорії {category!.Name}.", before, after);

            return "ok";
        }

        public async Task<string> DeleteFeatureAsync(int id, int initiatorId)
        {
            var feature = await _repository.GetFeatureByIdAsync(id);

            if (feature == null)
                return "not_found";

            await _repository.DeleteFeatureAsync(feature);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var category = await _repository.GetCategoryByIdAsync(feature.CategoryId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) ознаку {feature.Name} до категорії {category!.Name}.");

            return "ok";
        }

        public async Task<List<ResponseFeatureHeaderModel>> GetFeatureHeadersAsync(int categoryId)
        {
            var headers = await _repository.GetFeatureHeadersAsync(categoryId);

            return headers.Select(h => new ResponseFeatureHeaderModel
            {
                Id = h.Id,
                CategoryId = h.CategoryId,
                Name = h.Name,
                IsActive = h.IsActive,
                ViewOrder = h.ViewOrder
            }).ToList();
        }

        public async Task<string> AddFeatureHeaderAsync(AddFeatureHeaderModel model, int initiatorId)
        {
            await _repository.AddFeatureHeaderAsync(new FeatureHeaderEntity
            {
                CategoryId = model.CategoryId,
                Name = model.Name,
                ViewOrder = model.ViewOrder
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var category = await _repository.GetCategoryByIdAsync(model.CategoryId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) заголовок {model.Name} до категорії {category!.Name}.");

            return "ok";
        }

        public async Task<string> UpdateFeatureHeaderAsync(int id, UpdateFeatureHeaderModel model, int initiatorId)
        {
            var header = await _repository.GetFeatureHeaderByIdAsync(id);

            if (header == null)
                return "not_found";

            var before = JsonSerializer.Serialize(new { header.CategoryId, header.Name, header.IsActive, header.ViewOrder });

            header.CategoryId = model.CategoryId;
            header.Name = model.Name;
            header.IsActive = model.IsActive;
            header.ViewOrder = model.ViewOrder;
            await _repository.UpdateFeatureHeaderAsync(header);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = JsonSerializer.Serialize(new { model.CategoryId, model.Name, model.IsActive, model.ViewOrder });
            var category = await _repository.GetCategoryByIdAsync(model.CategoryId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) заголовок {header.Name} до категорії {category!.Name}.", before, after);

            return "ok";
        }

        public async Task<string> DeleteFeatureHeaderAsync(int id, int initiatorId)
        {
            var header = await _repository.GetFeatureHeaderByIdAsync(id);

            if (header == null)
                return "not_found";

            await _repository.DeleteFeatureHeaderAsync(header);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var category = await _repository.GetCategoryByIdAsync(header.CategoryId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) заголовок {header.Name} до категорії {category!.Name}.");

            return "ok";
        }

        public async Task<List<ResponseFilterValueModel>> GetFilterValuesAsync(int categoryId)
        {
            var values = await _repository.GetFilterValuesAsync(categoryId);

            return values.Select(v => new ResponseFilterValueModel
            {
                Id = v.Id,
                CategoryId = v.CategoryId,
                FeatureId = v.FeatureId,
                HeaderId = v.HeaderId,
                Value = v.Value,
                ValueSlug = v.ValueSlug,
                IsActive = v.IsActive,
                ViewOrder = v.ViewOrder
            }).ToList();
        }

        public async Task<string> AddFilterValueAsync(AddFilterValueModel model, int initiatorId)
        {
            await _repository.AddFilterValueAsync(new FilterValueEntity
            {
                CategoryId = model.CategoryId,
                FeatureId = model.FeatureId,
                HeaderId = model.HeaderId,
                Value = model.Value,
                ValueSlug = model.ValueSlug,
                IsActive = model.IsActive,
                ViewOrder = model.ViewOrder
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var category = await _repository.GetCategoryByIdAsync(model.CategoryId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) значення фільтру {model.Value} до категорії {category!.Name}.");


            return "ok";
        }

        public async Task<string> UpdateFilterValueAsync(int id, UpdateFilterValueModel model, int initiatorId)
        {
            var value = await _repository.GetFilterValueByIdAsync(id);

            if (value == null)
                return "not_found";

            var before = JsonSerializer.Serialize(new { value.FeatureId, value.Value, value.ValueSlug, value.IsActive, value.ViewOrder });

            value.CategoryId = model.CategoryId;
            value.FeatureId = model.FeatureId;
            value.HeaderId = model.HeaderId;
            value.Value = model.Value;
            value.ValueSlug = model.ValueSlug;
            value.IsActive = model.IsActive;
            value.ViewOrder = model.ViewOrder;
            await _repository.UpdateFilterValueAsync(value);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var category = await _repository.GetCategoryByIdAsync(model.CategoryId);
            var after = JsonSerializer.Serialize(new { model.FeatureId, model.Value, model.ValueSlug, model.IsActive, model.ViewOrder });
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) значення фільтру {value.Value} до категорії {category!.Name}.", before, after);

            return "ok";
        }

        public async Task<string> DeleteFilterValueAsync(int id, int initiatorId)
        {
            var value = await _repository.GetFilterValueByIdAsync(id);

            if (value == null)
                return "not_found";

            await _repository.DeleteFilterValueAsync(value);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var category = await _repository.GetCategoryByIdAsync(value.CategoryId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) значення фільтру {value.Value} до категорії {category!.Name}.");

            return "ok";
        }

        public async Task<List<ResponseDiscountGroupModel>> GetDiscountGroupsAsync()
        {
            var groups = await _repository.GetDiscountGroupsAsync();

            return groups.Select(g => new ResponseDiscountGroupModel
            {
                Id = g.Id,
                Name = g.Name,
                StartDate = g.StartDate,
                EndDate = g.EndDate,
                IsActive = g.IsActive
            }).ToList();
        }

        public async Task<string> AddDiscountGroupAsync(AddDiscountGroupModel model, int initiatorId)
        {
            await _repository.AddDiscountGroupAsync(new DiscountGroupEntity
            {
                Name = model.Name,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                IsActive = model.IsActive
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) створив(ла) групу знижок {model.Name}.");

            return "ok";
        }

        public async Task<string> UpdateDiscountGroupAsync(int id, UpdateDiscountGroupModel model, int initiatorId)
        {
            var group = await _repository.GetDiscountGroupByIdAsync(id);

            if (group == null)
                return "not_found";

            var before = JsonSerializer.Serialize(new { group.Name, group.StartDate, group.EndDate, group.IsActive });

            group.Name = model.Name;
            group.StartDate = model.StartDate;
            group.EndDate = model.EndDate;
            group.IsActive = model.IsActive;
            await _repository.UpdateDiscountGroupAsync(group);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = JsonSerializer.Serialize(new { model.Name, model.StartDate, model.EndDate, model.IsActive });
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) групу знижок {group.Name}.", before, after);

            return "ok";
        }

        public async Task<string> DeleteDiscountGroupAsync(int id, int initiatorId)
        {
            var group = await _repository.GetDiscountGroupByIdAsync(id);

            if (group == null)
                return "not_found";

            await _repository.DeleteDiscountGroupAsync(group);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) групу знижок {group.Name}.");

            return "ok";
        }

        public async Task<List<ResponseDiscountModel>> GetDiscountsAsync(int groupId)
        {
            var discounts = await _repository.GetDiscountsAsync(groupId);

            return discounts.Select(d => new ResponseDiscountModel
            {
                Id = d.Id,
                GroupId = d.GroupId,
                ProductId = d.ProductId,
                DiscountPercent = d.DiscountPercent
            }).ToList();
        }

        public async Task<string> AddDiscountAsync(AddDiscountModel model, int initiatorId)
        {
            var groupExists = await _repository.GetDiscountGroupByIdAsync(model.GroupId);

            if (groupExists == null)
                return "group_not_found";

            var discountExists = await _repository.DiscountExistsAsync(model.GroupId, model.ProductId);

            if (discountExists)
                return "discount_exists";

            await _repository.AddDiscountAsync(new DiscountEntity
            {
                GroupId = model.GroupId,
                ProductId = model.ProductId,
                DiscountPercent = model.DiscountPercent
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var product = await _repository.GetProductByIdAsync(model.ProductId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) знижку {model.DiscountPercent}% для товару {product!.Name} в групі {groupExists.Name}.");

            return "ok";
        }

        public async Task<string> UpdateDiscountAsync(int id, UpdateDiscountModel model, int initiatorId)
        {
            var discount = await _repository.GetDiscountByIdAsync(id);

            if (discount == null)
                return "not_found";

            var before = discount.DiscountPercent.ToString();

            discount.DiscountPercent = model.DiscountPercent;
            await _repository.UpdateDiscountAsync(discount);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = model.DiscountPercent.ToString();
            var product = await _repository.GetProductByIdAsync(discount.ProductId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) знижку для товару {product!.Name}.", before, after);

            return "ok";
        }

        public async Task<string> DeleteDiscountAsync(int id, int initiatorId)
        {
            var discount = await _repository.GetDiscountByIdAsync(id);

            if (discount == null)
                return "not_found";

            await _repository.DeleteDiscountAsync(discount);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var product = await _repository.GetProductByIdAsync(discount.ProductId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) знижку для товару {product!.Name}.");

            return "ok";
        }

        public async Task ReorderProductFeaturesAsync(int productId, List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetProductFeaturesOrderAsync(productId);
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderProductFeaturesAsync(items);
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var product = await _repository.GetProductByIdAsync(productId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок характеристик товару {product!.Name}.", before, after);
        }

        public async Task ReorderProductImagesAsync(int productId, List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetProductImagesOrderAsync(productId);
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderProductImagesAsync(items);
            _cache.Remove(CacheKeys.Top);
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var product = await _repository.GetProductByIdAsync(productId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок зображень товару {product!.Name}.", before, after);
        }

        public async Task ReorderFilterValuesAsync(int categoryId, List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetFilterValuesOrderAsync(categoryId);
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderFilterValuesAsync(items);
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var category = await _repository.GetCategoryByIdAsync(categoryId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок значень фільтрів категорії {category!.Name}.", before, after);
        }

        public async Task ReorderFeatureHeadersAsync(int categoryId, List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetFeatureHeadersOrderAsync(categoryId);
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderFeatureHeadersAsync(items);
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var category = await _repository.GetCategoryByIdAsync(categoryId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок заголовків характеристик категорії {category!.Name}.", before, after);
        }

        public async Task ReorderFeaturesAsync(int categoryId, List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetFeaturesOrderAsync(categoryId);
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderFeaturesAsync(items);
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var category = await _repository.GetCategoryByIdAsync(categoryId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок характеристик категорії {category!.Name}.", before, after);
        }

        public async Task ReorderDiscountGroupsAsync(List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetDiscountGroupsOrderAsync();
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderDiscountGroupsAsync(items);
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок груп знижок.", before, after);
        }
    }
}