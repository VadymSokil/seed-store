using Microsoft.Extensions.Caching.Memory;
using SeedStore.Admin.Catalog.Interfaces;
using SeedStore.Admin.Catalog.Models;
using SeedStore.Database.Entities.Store.Catalog;
using SeedStore.Support.Admin.EmployeesActivity.Interfaces;
using SeedStore.Support.Admin.Reorder.Models;
using SeedStore.Support.General.Constants.CacheKeys;
using System.Text.Json;

namespace SeedStore.Admin.Catalog.Services
{
    public class AdminCatalogService : IAdminCatalogService
    {
        private readonly IAdminCatalogRepository _repository;
        private readonly IEmployeesActivityService _employeesActivityService;
        private readonly IMemoryCache _cache;

        public AdminCatalogService(IAdminCatalogRepository repository, IEmployeesActivityService employeesActivityService, IMemoryCache cache)
        {
            _repository = repository;
            _employeesActivityService = employeesActivityService;
            _cache = cache;
        }

        public async Task<List<ResponseCategoriesModel>> GetCategoriesAsync()
        {
            if (_cache.TryGetValue(CacheKeys.AdminCategories, out List<ResponseCategoriesModel> cached))
                return cached;

            var categories = await _repository.GetCategoriesAsync();

            var result = categories.Select(c => new ResponseCategoriesModel
            {
                Id = c.Id,
                ParentId = c.ParentId,
                Name = c.Name,
                Slug = c.Slug,
                ImageUrl = c.ImageUrl,
                IsActive = c.IsActive,
                ViewOrder = c.ViewOrder
            }).ToList();

            _cache.Set(CacheKeys.AdminCategories, result, TimeSpan.FromHours(1));
            return result;
        }

        public async Task<ResponseCategoryModel?> GetCategoryAsync(int id)
        {
            var category = await _repository.GetCategoryByIdAsync(id);

            if (category == null)
                return null;

            return await BuildCategoryTreeAsync(category);
        }

        private async Task<ResponseCategoryModel> BuildCategoryTreeAsync(CategoryEntity category)
        {
            var model = new ResponseCategoryModel
            {
                Id = category.Id,
                ParentId = category.ParentId,
                Name = category.Name,
                Slug = category.Slug,
                ImageUrl = category.ImageUrl,
                IsActive = category.IsActive,
                ViewOrder = category.ViewOrder
            };

            var children = await _repository.GetChildCategoriesAsync(category.Id);

            foreach (var child in children)
            {
                model.Children.Add(await BuildCategoryTreeAsync(child));
            }

            return model;
        }

        public async Task<string> AddCategoryAsync(AddCategoryModel model, int initiatorId)
        {
            var slugExists = await _repository.SlugExistsAsync(model.Slug);

            if (slugExists)
                return "slug_taken";

            await _repository.AddCategoryAsync(new CategoryEntity
            {
                ParentId = model.ParentId,
                Name = model.Name,
                Slug = model.Slug,
                ImageUrl = model.ImageUrl,
                IsActive = model.IsActive,
                ViewOrder = model.ViewOrder
            });

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) додав(ла) категорію {model.Name}.");

            _cache.Remove(CacheKeys.StoreCategories);
            _cache.Remove(CacheKeys.AdminCategories);
            return "ok";
        }

        public async Task<string> UpdateCategoryAsync(int id, UpdateCategoryModel model, int initiatorId)
        {
            var category = await _repository.GetCategoryByIdAsync(id);

            if (category == null)
                return "not_found";

            var slugExists = await _repository.SlugExistsAsync(model.Slug);

            if (slugExists && category.Slug != model.Slug)
                return "slug_taken";

            var before = JsonSerializer.Serialize(new { category.ParentId, category.Name, category.Slug, category.ImageUrl, category.IsActive, category.ViewOrder });

            category.ParentId = model.ParentId;
            category.Name = model.Name;
            category.Slug = model.Slug;
            category.ImageUrl = model.ImageUrl;
            category.IsActive = model.IsActive;
            category.ViewOrder = model.ViewOrder;
            await _repository.UpdateCategoryAsync(category);

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = JsonSerializer.Serialize(new { model.ParentId, model.Name, model.Slug, model.ImageUrl, model.IsActive, model.ViewOrder });
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) оновив(ла) категорію {category.Name}.", before, after);

            _cache.Remove(CacheKeys.StoreCategories);
            _cache.Remove(CacheKeys.AdminCategories);
            return "ok";
        }

        public async Task<string> DeleteCategoryAsync(int id, int initiatorId)
        {
            var category = await _repository.GetCategoryByIdAsync(id);
            if (category == null)
                return "not_found";

            var children = await _repository.GetChildCategoriesAsync(id);
            var childrenNames = children.Any()
                ? string.Join(", ", children.Select(c => c.Name))
                : "немає";

            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            var after = children.Any() ? $"Дочірні категорії: {childrenNames}" : null;
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) видалив(ла) категорію {category.Name}.", null, after);

            await _repository.DeleteCategoryAsync(category);
            _cache.Remove(CacheKeys.StoreCategories);
            _cache.Remove(CacheKeys.AdminCategories);
            return "ok";
        }

        public async Task ReorderCategoriesAsync(List<ReorderItemModel> items, int initiatorId)
        {
            var currentOrder = await _repository.GetCategoriesOrderAsync();
            var before = JsonSerializer.Serialize(currentOrder);
            await _repository.ReorderCategoriesAsync(items);
            _cache.Remove(CacheKeys.StoreCategories);
            _cache.Remove(CacheKeys.AdminCategories);
            var after = JsonSerializer.Serialize(items);
            var initiator = await _repository.GetEmployeeByIdAsync(initiatorId);
            await _employeesActivityService.LogAsync(initiatorId, $"{initiator!.Role!.Name} {initiator.Name}({initiatorId}) змінив(ла) порядок категорій.", before, after);
        }
    }
}