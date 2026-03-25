using Microsoft.Extensions.Caching.Memory;
using SeedStore.Store.Catalog.Interfaces;
using SeedStore.Store.Catalog.Models;
using SeedStore.Support.General.Constants.CacheKeys;

namespace SeedStore.Store.Catalog.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _categoryRepository;
        private readonly IMemoryCache _cache;

        public CatalogService(ICatalogRepository categoryRepository, IMemoryCache cache)
        {
            _categoryRepository = categoryRepository;
            _cache = cache;
        }

        public async Task<List<CategoryResponseModel>> GetCategoriesAsync()
        {
            if (_cache.TryGetValue(CacheKeys.StoreCategories, out List<CategoryResponseModel> cached))
                return cached;

            var categories = await _categoryRepository.GetCategoriesAsync();
            var result = categories.Select(c => new CategoryResponseModel
            {
                Id = c.Id,
                ParentId = c.ParentId,
                Name = c.Name,
                Slug = c.Slug,
                ImageUrl = c.ImageUrl,
                ViewOrder = c.ViewOrder
            }).ToList();

            _cache.Set(CacheKeys.StoreCategories, result, TimeSpan.FromHours(1));
            return result;
        }

        public async Task<List<CategorySearchResponseModel>> SearchCategoriesAsync(string value)
        {
            var categories = await _categoryRepository.SearchCategoriesAsync(value);

            return categories.Select(c => new CategorySearchResponseModel
            {
                Name = c.Name
            }).ToList();
        }
    }
}
