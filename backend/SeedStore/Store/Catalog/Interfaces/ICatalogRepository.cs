using SeedStore.Database.Entities.Store.Catalog;
using SeedStore.Store.Catalog.Models;

namespace SeedStore.Store.Catalog.Interfaces
{
    public interface ICatalogRepository
    {
        Task<List<CategoryEntity>> GetCategoriesAsync();
        Task<List<CategorySearchResponseModel>> SearchCategoriesAsync(string value);
    }
}
