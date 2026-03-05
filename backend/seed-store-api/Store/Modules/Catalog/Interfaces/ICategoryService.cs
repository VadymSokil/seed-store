using seed_store_api.Store.Modules.Catalog.Models;

namespace seed_store_api.Store.Modules.Catalog.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponseModel>> GetCategoriesAsync();
        Task<List<CategorySearchResponseModel>> SearchCategoriesAsync(string value);
    }
}
