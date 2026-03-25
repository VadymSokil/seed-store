using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Store.Catalog;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Catalog.Interfaces
{
    public interface IAdminCatalogRepository
    {
        Task<List<CategoryEntity>> GetCategoriesAsync();
        Task<CategoryEntity?> GetCategoryByIdAsync(int id);
        Task<List<CategoryEntity>> GetChildCategoriesAsync(int parentId);
        Task<bool> SlugExistsAsync(string slug);
        Task AddCategoryAsync(CategoryEntity entity);
        Task UpdateCategoryAsync(CategoryEntity entity);
        Task DeleteCategoryAsync(CategoryEntity entity);
        Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId);
        Task ReorderCategoriesAsync(List<ReorderItemModel> items);
        Task<List<ReorderItemModel>> GetCategoriesOrderAsync();
    }
}
