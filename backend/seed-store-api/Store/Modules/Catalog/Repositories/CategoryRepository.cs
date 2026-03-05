using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Context;
using seed_store_api.Database.Entities.Store.Modules.Catalog;
using seed_store_api.Store.Modules.Catalog.Interfaces;

namespace seed_store_api.Store.Modules.Catalog.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryEntity>> GetCategoriesAsync()
        {
            return await _context.Categories.Where(c => c.IsActive).OrderBy(c => c.ViewOrder).ToListAsync();
        }

        public async Task<List<CategoryEntity>> SearchCategoriesAsync(string value)
        {
            return await _context.Categories.Where(c => c.IsActive && c.Name.ToLower().Contains(value.ToLower())).OrderBy(c => c.ViewOrder).ToListAsync();
        }
    }
}
