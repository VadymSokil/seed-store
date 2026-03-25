using SeedStore.Store.Catalog.Models;

namespace SeedStore.Store.Catalog.Interfaces
{
    public interface ICatalogService
    {
        Task<List<CategoryResponseModel>> GetCategoriesAsync();
        Task<List<CategorySearchResponseModel>> SearchCategoriesAsync(string value);
    }
}
