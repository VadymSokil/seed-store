using SeedStore.Database.Entities.Store.Catalog;

namespace SeedStore.Store.Catalog.Interfaces
{
    public interface ICatalogRepository
    {
        Task<List<CategoryEntity>> GetCategoriesAsync();
        Task<List<CategoryEntity>> SearchCategoriesAsync(string value);
    }
}
