using SeedStore.Admin.Catalog.Models;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Catalog.Interfaces
{
    public interface IAdminCatalogService
    {
        Task<List<ResponseCategoriesModel>> GetCategoriesAsync();
        Task<ResponseCategoryModel?> GetCategoryAsync(int id);
        Task<string> AddCategoryAsync(AddCategoryModel model, int initiatorId);
        Task<string> UpdateCategoryAsync(int id, UpdateCategoryModel model, int initiatorId);
        Task<string> DeleteCategoryAsync(int id, int initiatorId);
        Task ReorderCategoriesAsync(List<ReorderItemModel> items, int initiatorId);
    }
}
