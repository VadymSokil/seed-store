using Microsoft.EntityFrameworkCore;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Store.Catalog;
using SeedStore.Store.Catalog.Interfaces;

namespace SeedStore.Store.Catalog.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly AppDbContext _context;

        public CatalogRepository(AppDbContext context)
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
