using SeedStore.Store.Products.Models;

namespace SeedStore.Store.Products.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductSearchResponseModel>> SearchProductsAsync(string value);
        Task<ProductTopResponseModel> GetProductsTopAsync();
        Task<CategoryFiltersResponseModel> GetCategoryFiltersAsync(int id);
        Task<ProductListResponseModel> GetProductsListAsync(ProductListRequestModel productListRequest);
        Task<ProductDetailsModel?> GetProductDetailsAsync(string idOrSlug);
        Task<List<DiscountGroupResponseModel>> GetActiveDiscountGroupsAsync();
    }
}
