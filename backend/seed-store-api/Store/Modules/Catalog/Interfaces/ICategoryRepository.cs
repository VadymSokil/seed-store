using seed_store_api.Database.Entities.Store.Modules.Catalog;

namespace seed_store_api.Store.Modules.Catalog.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoryEntity>> GetCategoriesAsync();
        Task<List<CategoryEntity>> SearchCategoriesAsync(string value);
    }
}
