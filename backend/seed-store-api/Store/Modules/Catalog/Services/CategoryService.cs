using Microsoft.Extensions.Caching.Memory;
using seed_store_api.Store.Modules.Catalog.Interfaces;
using seed_store_api.Store.Modules.Catalog.Models;

namespace seed_store_api.Store.Modules.Catalog.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMemoryCache _cache;
        private const string CategoryCacheKey = "categories";

        public CategoryService(ICategoryRepository categoryRepository, IMemoryCache cache)
        {
            _categoryRepository = categoryRepository;
            _cache = cache;
        }

        public async Task<List<CategoryResponseModel>> GetCategoriesAsync()
        {
            if (_cache.TryGetValue(CategoryCacheKey, out List<CategoryResponseModel> cached))
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

            _cache.Set(CategoryCacheKey, result, TimeSpan.FromHours(1));
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
