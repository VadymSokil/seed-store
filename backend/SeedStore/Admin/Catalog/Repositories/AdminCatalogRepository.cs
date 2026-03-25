using Microsoft.EntityFrameworkCore;
using SeedStore.Admin.Catalog.Interfaces;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Store.Catalog;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Catalog.Repositories
{
    public class AdminCatalogRepository : IAdminCatalogRepository
    {
        private readonly AppDbContext _context;

        public AdminCatalogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryEntity>> GetCategoriesAsync()
        {
            return await _context.Categories.OrderBy(c => c.ViewOrder).ToListAsync();
        }

        public async Task<CategoryEntity?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<CategoryEntity>> GetChildCategoriesAsync(int parentId)
        {
            return await _context.Categories
                .Where(c => c.ParentId == parentId)
                .OrderBy(c => c.ViewOrder)
                .ToListAsync();
        }

        public async Task<bool> SlugExistsAsync(string slug)
        {
            return await _context.Categories.AnyAsync(c => c.Slug == slug);
        }

        public async Task AddCategoryAsync(CategoryEntity entity)
        {
            entity.ViewOrder = await GetNextViewOrderAsync();
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(CategoryEntity entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(CategoryEntity entity)
        {
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }
        public async Task<int> GetNextViewOrderAsync()
        {
            return (await _context.Categories.MaxAsync(c => c.ViewOrder) ?? 0) + 1;
        }

        public async Task ReorderCategoriesAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.Categories
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task<List<ReorderItemModel>> GetCategoriesOrderAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.ViewOrder)
                .Select(c => new ReorderItemModel { Id = c.Id, ViewOrder = c.ViewOrder ?? 0 })
                .ToListAsync();
        }
    }
}