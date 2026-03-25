using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Store.Catalog;
using SeedStore.Database.Entities.Store.Products;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Products.Interfaces
{
    public interface IAdminProductsRepository
    {
        Task<List<ProductEntity>> GetProductsAsync(int? categoryId, bool? isActive);
        Task<ProductEntity?> GetProductByIdAsync(int id);
        Task<bool> SlugExistsAsync(string slug);
        Task<bool> ArticleExistsAsync(string article);
        Task AddProductAsync(ProductEntity entity);
        Task UpdateProductAsync(ProductEntity entity);
        Task DeleteProductAsync(ProductEntity entity);

        Task<List<ProductImageEntity>> GetProductImagesAsync(int productId);
        Task<ProductImageEntity?> GetProductImageByIdAsync(int imageId);
        Task AddProductImageAsync(ProductImageEntity entity);
        Task UpdateProductImageAsync(ProductImageEntity entity);
        Task DeleteProductImageAsync(ProductImageEntity entity);

        Task<List<ProductFeatureEntity>> GetProductFeaturesAsync(int productId);
        Task<ProductFeatureEntity?> GetProductFeatureByIdAsync(int featureId);
        Task AddProductFeatureAsync(ProductFeatureEntity entity);
        Task UpdateProductFeatureAsync(ProductFeatureEntity entity);
        Task DeleteProductFeatureAsync(ProductFeatureEntity entity);

        Task<List<FeatureEntity>> GetFeaturesAsync(int categoryId);
        Task<FeatureEntity?> GetFeatureByIdAsync(int id);
        Task AddFeatureAsync(FeatureEntity entity);
        Task UpdateFeatureAsync(FeatureEntity entity);
        Task DeleteFeatureAsync(FeatureEntity entity);

        Task<List<FeatureHeaderEntity>> GetFeatureHeadersAsync(int categoryId);
        Task<FeatureHeaderEntity?> GetFeatureHeaderByIdAsync(int id);
        Task AddFeatureHeaderAsync(FeatureHeaderEntity entity);
        Task UpdateFeatureHeaderAsync(FeatureHeaderEntity entity);
        Task DeleteFeatureHeaderAsync(FeatureHeaderEntity entity);

        Task<List<FilterValueEntity>> GetFilterValuesAsync(int categoryId);
        Task<FilterValueEntity?> GetFilterValueByIdAsync(int id);
        Task AddFilterValueAsync(FilterValueEntity entity);
        Task UpdateFilterValueAsync(FilterValueEntity entity);
        Task DeleteFilterValueAsync(FilterValueEntity entity);

        Task<List<DiscountGroupEntity>> GetDiscountGroupsAsync();
        Task<DiscountGroupEntity?> GetDiscountGroupByIdAsync(int id);
        Task AddDiscountGroupAsync(DiscountGroupEntity entity);
        Task UpdateDiscountGroupAsync(DiscountGroupEntity entity);
        Task DeleteDiscountGroupAsync(DiscountGroupEntity entity);

        Task<List<DiscountEntity>> GetDiscountsAsync(int groupId);
        Task<DiscountEntity?> GetDiscountByIdAsync(int id);
        Task<bool> DiscountExistsAsync(int groupId, int productId);
        Task AddDiscountAsync(DiscountEntity entity);
        Task UpdateDiscountAsync(DiscountEntity entity);
        Task DeleteDiscountAsync(DiscountEntity entity);
        Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId);

        Task ReorderProductFeaturesAsync(List<ReorderItemModel> items);
        Task ReorderProductImagesAsync(List<ReorderItemModel> items);
        Task ReorderFilterValuesAsync(List<ReorderItemModel> items);
        Task ReorderFeatureHeadersAsync(List<ReorderItemModel> items);
        Task ReorderFeaturesAsync(List<ReorderItemModel> items);
        Task ReorderDiscountGroupsAsync(List<ReorderItemModel> items);

        Task<List<ReorderItemModel>> GetProductFeaturesOrderAsync(int productId);
        Task<List<ReorderItemModel>> GetProductImagesOrderAsync(int productId);
        Task<List<ReorderItemModel>> GetFilterValuesOrderAsync(int categoryId);
        Task<List<ReorderItemModel>> GetFeatureHeadersOrderAsync(int categoryId);
        Task<List<ReorderItemModel>> GetFeaturesOrderAsync(int categoryId);
        Task<List<ReorderItemModel>> GetDiscountGroupsOrderAsync();

        Task<CategoryEntity?> GetCategoryByIdAsync(int id);
    }
}
