using Microsoft.Extensions.Caching.Memory;
using SeedStore.Store.Products.Interfaces;
using SeedStore.Store.Products.Models;
using SeedStore.Support.General.Constants.CacheKeys;

namespace SeedStore.Store.Products.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMemoryCache _cache;
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
                Slug = c.Slug,
                Price = c.Price,
                ImageUrl = c.ImageUrl,
            }).ToList();
        }

        public async Task<ProductTopResponseModel> GetProductsTopAsync()
        {
            if (_cache.TryGetValue(CacheKeys.Top, out ProductTopResponseModel cached))
                return cached;

            var result = await _productRepository.GetProductsTopAsync();
            _cache.Set(CacheKeys.Top, result, TimeSpan.FromHours(1));
            return result;
        }

        public async Task<CategoryFiltersResponseModel> GetCategoryFiltersAsync(int id)
        {
            return await _productRepository.GetCategoryFiltersAsync(id);
        }

        public async Task<ProductListResponseModel> GetProductsListAsync(ProductListRequestModel request)
        {
            return await _productRepository.GetProductsListAsync(request);
        }

        public async Task<ProductDetailsModel?> GetProductDetailsAsync(string idOrSlug)
        {
            return await _productRepository.GetProductDetailsAsync(idOrSlug);
        }
    }
}
