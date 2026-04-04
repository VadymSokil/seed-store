using Microsoft.EntityFrameworkCore;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Store.Catalog;
using SeedStore.Store.Catalog.Interfaces;
using SeedStore.Store.Catalog.Models;

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

        public async Task<List<CategorySearchResponseModel>> SearchCategoriesAsync(string value)
        {
            var allCategories = await _context.Categories
                .Where(c => c.IsActive)
                .ToListAsync();

            var matched = allCategories
                .Where(c => c.Name.ToLower().Contains(value.ToLower()))
                .OrderBy(c => c.ViewOrder)
                .ToList();

            return matched.Select(c =>
            {
                var slugParts = new List<string>();
                var current = c;
                while (current != null)
                {
                    slugParts.Insert(0, current.Slug);
                    current = current.ParentId.HasValue
                        ? allCategories.FirstOrDefault(x => x.Id == current.ParentId.Value)
                        : null;
                }
                return new CategorySearchResponseModel
                {
                    Name = c.Name,
                    Slug = c.Slug,
                    Path = string.Join("/", slugParts),
                };
            }).ToList();
        }
    }
}
