using seed_store_api.Store.Modules.Products.Models;

namespace seed_store_api.Store.Modules.Products.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductSearchResponseModel>> SearchProductsAsync(string value);
        Task<ProductTopResponseModel> GetProductsTopAsync();
        Task<CategoryFiltersResponseModel> GetCategoryFiltersAsync(int id);
        Task<List<ProductCardModel>> GetProductsListAsync(ProductListRequestModel productListRequest);
        Task<ProductDetailsModel?> GetProductDetailsAsync(string idOrSlug);
    }
}
