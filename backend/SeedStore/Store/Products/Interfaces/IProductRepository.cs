using SeedStore.Store.Products.Models;

namespace SeedStore.Store.Products.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductSearchResponseModel>> SearchProductsAsync(string value);
        Task<ProductTopResponseModel> GetProductsTopAsync();
        Task<CategoryFiltersResponseModel> GetCategoryFiltersAsync(int id);
        Task<ProductListResponseModel> GetProductsListAsync(ProductListRequestModel request);
        Task<ProductDetailsModel?> GetProductDetailsAsync(string idOrSlug);
        Task RecalculateProductRatingAsync(int productId);
    }
}
