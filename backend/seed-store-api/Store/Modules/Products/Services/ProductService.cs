using Microsoft.Extensions.Caching.Memory;
using seed_store_api.Store.Modules.Products.Interfaces;
using seed_store_api.Store.Modules.Products.Models;

namespace seed_store_api.Store.Modules.Products.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;
        private const string TopCacheKey = "top";
        private const string FilterCasheKey = "filter";
        public ProductService(IProductRepository productRepository, IMemoryCache cache)
        {
            _productRepository = productRepository;
            _cache = cache;
        }

        public async Task<List<ProductSearchResponseModel>> SearchProductsAsync(string value)
        {
            var products = await _productRepository.SearchProductsAsync(value);

            return products.Select(c => new ProductSearchResponseModel
            {
                Name = c.Name,
                Price = c.Price,
                ImageUrl = c.ImageUrl,
            }).ToList();
        }

        public async Task<ProductTopResponseModel> GetProductsTopAsync()
        {
            if (_cache.TryGetValue(TopCacheKey, out ProductTopResponseModel cached))
                return cached;

            var result = await _productRepository.GetProductsTopAsync();
            _cache.Set(TopCacheKey, result, TimeSpan.FromHours(1));
            return result;
        }

        public async Task<CategoryFiltersResponseModel> GetCategoryFiltersAsync(int id)
        {
            var cacheKey = $"{FilterCasheKey}_{id}";

            if (_cache.TryGetValue(cacheKey, out CategoryFiltersResponseModel cached))
                return cached;

            var result = await _productRepository.GetCategoryFiltersAsync(id);
            _cache.Set(cacheKey, result, TimeSpan.FromHours(1));
            return result;
        }

        public async Task<List<ProductCardModel>> GetProductsListAsync(ProductListRequestModel request)
        {
            return await _productRepository.GetProductsListAsync(request);
        }

        public async Task<ProductDetailsModel?> GetProductDetailsAsync(string idOrSlug)
        {
            return await _productRepository.GetProductDetailsAsync(idOrSlug);
        }
    }
}
