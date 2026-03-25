using SeedStore.Admin.Products.Models;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Products.Interfaces
{
    public interface IAdminProductsService
    {
        Task<List<ResponseProductsModel>> GetProductsAsync(int? categoryId, bool? isActive);
        Task<ResponseProductModel?> GetProductAsync(int id);
        Task<string> AddProductAsync(AddProductModel model, int initiatorId);
        Task<string> UpdateProductAsync(int id, UpdateProductModel model, int initiatorId);
        Task<string> DeleteProductAsync(int id, int initiatorId);

        Task<List<ResponseProductImageModel>> GetProductImagesAsync(int productId);
        Task<string> AddProductImageAsync(int productId, AddProductImageModel model, int initiatorId);
        Task<string> UpdateProductImageAsync(int imageId, UpdateProductImageModel model, int initiatorId);
        Task<string> DeleteProductImageAsync(int imageId, int initiatorId);

        Task<List<ResponseProductFeatureModel>> GetProductFeaturesAsync(int productId);
        Task<string> AddProductFeatureAsync(int productId, AddProductFeatureModel model, int initiatorId);
        Task<string> UpdateProductFeatureAsync(int featureId, UpdateProductFeatureModel model, int initiatorId);
        Task<string> DeleteProductFeatureAsync(int featureId, int initiatorId);

        Task<List<ResponseFeatureModel>> GetFeaturesAsync(int categoryId);
        Task<string> AddFeatureAsync(AddFeatureModel model, int initiatorId);
        Task<string> UpdateFeatureAsync(int id, UpdateFeatureModel model, int initiatorId);
        Task<string> DeleteFeatureAsync(int id, int initiatorId);

        Task<List<ResponseFeatureHeaderModel>> GetFeatureHeadersAsync(int categoryId);
        Task<string> AddFeatureHeaderAsync(AddFeatureHeaderModel model, int initiatorId);
        Task<string> UpdateFeatureHeaderAsync(int id, UpdateFeatureHeaderModel model, int initiatorId);
        Task<string> DeleteFeatureHeaderAsync(int id, int initiatorId);

        Task<List<ResponseFilterValueModel>> GetFilterValuesAsync(int categoryId);
        Task<string> AddFilterValueAsync(AddFilterValueModel model, int initiatorId);
        Task<string> UpdateFilterValueAsync(int id, UpdateFilterValueModel model, int initiatorId);
        Task<string> DeleteFilterValueAsync(int id, int initiatorId);

        Task<List<ResponseDiscountGroupModel>> GetDiscountGroupsAsync();
        Task<string> AddDiscountGroupAsync(AddDiscountGroupModel model, int initiatorId);
        Task<string> UpdateDiscountGroupAsync(int id, UpdateDiscountGroupModel model, int initiatorId);
        Task<string> DeleteDiscountGroupAsync(int id, int initiatorId);

        Task<List<ResponseDiscountModel>> GetDiscountsAsync(int groupId);
        Task<string> AddDiscountAsync(AddDiscountModel model, int initiatorId);
        Task<string> UpdateDiscountAsync(int id, UpdateDiscountModel model, int initiatorId);
        Task<string> DeleteDiscountAsync(int id, int initiatorId);

        Task ReorderProductFeaturesAsync(int productId, List<ReorderItemModel> items, int initiatorId);
        Task ReorderProductImagesAsync(int productId, List<ReorderItemModel> items, int initiatorId);
        Task ReorderFilterValuesAsync(int categoryId, List<ReorderItemModel> items, int initiatorId);
        Task ReorderFeatureHeadersAsync(int categoryId, List<ReorderItemModel> items, int initiatorId);
        Task ReorderFeaturesAsync(int categoryId, List<ReorderItemModel> items, int initiatorId);
        Task ReorderDiscountGroupsAsync(List<ReorderItemModel> items, int initiatorId);
    }
}
