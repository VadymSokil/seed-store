using Microsoft.EntityFrameworkCore;
using SeedStore.Admin.Products.Interfaces;
using SeedStore.Database.Context;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Store.Catalog;
using SeedStore.Database.Entities.Store.Products;
using SeedStore.Support.Admin.Reorder.Models;

namespace SeedStore.Admin.Products.Repositories
{
    public class AdminProductsRepository : IAdminProductsRepository
    {
        private readonly AppDbContext _context;

        public AdminProductsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductEntity>> GetProductsAsync(int? categoryId, bool? isActive)
        {
            var query = _context.Products.AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId);

            if (isActive.HasValue)
                query = query.Where(p => p.IsActive == isActive);

            return await query.OrderByDescending(p => p.CreatedDate).ToListAsync();
        }

        public async Task<ProductEntity?> GetProductByIdAsync(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> SlugExistsAsync(string slug)
        {
            return await _context.Products.AnyAsync(p => p.Slug == slug);
        }

        public async Task<bool> ArticleExistsAsync(string article)
        {
            return await _context.Products.AnyAsync(p => p.Article == article);
        }

        public async Task AddProductAsync(ProductEntity entity)
        {
            await _context.Products.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(ProductEntity entity)
        {
            _context.Products.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(ProductEntity entity)
        {
            _context.Products.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductImageEntity>> GetProductImagesAsync(int productId)
        {
            return await _context.ProductImages
                .Where(i => i.ProductId == productId)
                .OrderBy(i => i.ViewOrder)
                .ToListAsync();
        }

        public async Task<ProductImageEntity?> GetProductImageByIdAsync(int imageId)
        {
            return await _context.ProductImages.FirstOrDefaultAsync(i => i.Id == imageId);
        }

        public async Task AddProductImageAsync(ProductImageEntity entity)
        {
            entity.ViewOrder = await GetNextProductImageViewOrderAsync();
            await _context.ProductImages.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductImageAsync(ProductImageEntity entity)
        {
            _context.ProductImages.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ProductFeatureEntity>> GetProductFeaturesAsync(int productId)
        {
            return await _context.ProductFeatures
                .Include(f => f.Feature)
                .Include(f => f.Header)
                .Where(f => f.ProductId == productId)
                .OrderBy(f => f.ViewOrder)
                .ToListAsync();
        }

        public async Task UpdateProductImageAsync(ProductImageEntity entity)
        {
            _context.ProductImages.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductFeatureEntity?> GetProductFeatureByIdAsync(int featureId)
        {
            return await _context.ProductFeatures
                .Include(f => f.Feature)
                .Include(f => f.Header)
                .FirstOrDefaultAsync(f => f.Id == featureId);
        }

        public async Task AddProductFeatureAsync(ProductFeatureEntity entity)
        {
            entity.ViewOrder = await GetNextProductFeatureViewOrderAsync();
            await _context.ProductFeatures.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductFeatureAsync(ProductFeatureEntity entity)
        {
            _context.ProductFeatures.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductFeatureAsync(ProductFeatureEntity entity)
        {
            _context.ProductFeatures.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FeatureEntity>> GetFeaturesAsync(int categoryId)
        {
            return await _context.Features
                .Where(f => f.CategoryId == categoryId)
                .OrderBy(f => f.ViewOrder)
                .ToListAsync();
        }

        public async Task<FeatureEntity?> GetFeatureByIdAsync(int id)
        {
            return await _context.Features.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AddFeatureAsync(FeatureEntity entity)
        {
            entity.ViewOrder = await GetNextFeatureViewOrderAsync();
            await _context.Features.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeatureAsync(FeatureEntity entity)
        {
            _context.Features.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFeatureAsync(FeatureEntity entity)
        {
            _context.Features.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FeatureHeaderEntity>> GetFeatureHeadersAsync(int categoryId)
        {
            return await _context.FeatureHeaders
                .Where(h => h.CategoryId == categoryId)
                .OrderBy(h => h.ViewOrder)
                .ToListAsync();
        }

        public async Task<FeatureHeaderEntity?> GetFeatureHeaderByIdAsync(int id)
        {
            return await _context.FeatureHeaders.FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task AddFeatureHeaderAsync(FeatureHeaderEntity entity)
        {
            entity.ViewOrder = await GetNextFeatureHeaderViewOrderAsync();
            await _context.FeatureHeaders.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFeatureHeaderAsync(FeatureHeaderEntity entity)
        {
            _context.FeatureHeaders.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFeatureHeaderAsync(FeatureHeaderEntity entity)
        {
            _context.FeatureHeaders.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FilterValueEntity>> GetFilterValuesAsync(int categoryId)
        {
            return await _context.FilterValues
                .Where(f => f.CategoryId == categoryId)
                .OrderBy(f => f.ViewOrder)
                .ToListAsync();
        }

        public async Task<FilterValueEntity?> GetFilterValueByIdAsync(int id)
        {
            return await _context.FilterValues.FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task AddFilterValueAsync(FilterValueEntity entity)
        {
            entity.ViewOrder = await GetNextFilterValueViewOrderAsync();
            await _context.FilterValues.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateFilterValueAsync(FilterValueEntity entity)
        {
            _context.FilterValues.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFilterValueAsync(FilterValueEntity entity)
        {
            _context.FilterValues.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DiscountGroupEntity>> GetDiscountGroupsAsync()
        {
            return await _context.DiscountGroups.OrderByDescending(g => g.StartDate).ToListAsync();
        }

        public async Task<DiscountGroupEntity?> GetDiscountGroupByIdAsync(int id)
        {
            return await _context.DiscountGroups.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task AddDiscountGroupAsync(DiscountGroupEntity entity)
        {
            entity.ViewOrder = await GetNextDiscountGroupViewOrderAsync();
            await _context.DiscountGroups.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDiscountGroupAsync(DiscountGroupEntity entity)
        {
            _context.DiscountGroups.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDiscountGroupAsync(DiscountGroupEntity entity)
        {
            _context.DiscountGroups.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<DiscountEntity>> GetDiscountsAsync(int groupId)
        {
            return await _context.Discounts
                .Where(d => d.GroupId == groupId)
                .ToListAsync();
        }

        public async Task<DiscountEntity?> GetDiscountByIdAsync(int id)
        {
            return await _context.Discounts.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> DiscountExistsAsync(int groupId, int productId)
        {
            return await _context.Discounts.AnyAsync(d => d.GroupId == groupId && d.ProductId == productId);
        }

        public async Task AddDiscountAsync(DiscountEntity entity)
        {
            await _context.Discounts.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDiscountAsync(DiscountEntity entity)
        {
            _context.Discounts.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDiscountAsync(DiscountEntity entity)
        {
            _context.Discounts.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeEntity?> GetEmployeeByIdAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        private async Task<int> GetNextProductFeatureViewOrderAsync()
        {
            return (await _context.ProductFeatures.MaxAsync(p => p.ViewOrder) ?? 0) + 1;
        }

        private async Task<int> GetNextProductImageViewOrderAsync()
        {
            return (await _context.ProductImages.MaxAsync(p => p.ViewOrder) ?? 0) + 1;
        }

        private async Task<int> GetNextFilterValueViewOrderAsync()
        {
            return (await _context.FilterValues.MaxAsync(p => p.ViewOrder) ?? 0) + 1;
        }

        private async Task<int> GetNextFeatureHeaderViewOrderAsync()
        {
            return (await _context.FeatureHeaders.MaxAsync(p => p.ViewOrder) ?? 0) + 1;
        }

        private async Task<int> GetNextFeatureViewOrderAsync()
        {
            return (await _context.Features.MaxAsync(p => p.ViewOrder) ?? 0) + 1;
        }

        private async Task<int> GetNextDiscountGroupViewOrderAsync()
        {
            return (await _context.DiscountGroups.MaxAsync(p => p.ViewOrder) ?? 0) + 1;
        }

        public async Task ReorderProductFeaturesAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.ProductFeatures
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task ReorderProductImagesAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.ProductImages
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task ReorderFilterValuesAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.FilterValues
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task ReorderFeatureHeadersAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.FeatureHeaders
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task ReorderFeaturesAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.Features
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task ReorderDiscountGroupsAsync(List<ReorderItemModel> items)
        {
            foreach (var item in items)
            {
                await _context.DiscountGroups
                    .Where(c => c.Id == item.Id)
                    .ExecuteUpdateAsync(s => s.SetProperty(c => c.ViewOrder, item.ViewOrder));
            }
        }

        public async Task<List<ReorderItemModel>> GetProductFeaturesOrderAsync(int productId)
        {
            return await _context.ProductFeatures
                .Where(f => f.ProductId == productId)
                .OrderBy(f => f.ViewOrder)
                .Select(f => new ReorderItemModel { Id = f.Id, ViewOrder = f.ViewOrder ?? 0 })
                .ToListAsync();
        }

        public async Task<List<ReorderItemModel>> GetProductImagesOrderAsync(int productId)
        {
            return await _context.ProductImages
                .Where(i => i.ProductId == productId)
                .OrderBy(i => i.ViewOrder)
                .Select(i => new ReorderItemModel { Id = i.Id, ViewOrder = i.ViewOrder ?? 0 })
                .ToListAsync();
        }

        public async Task<List<ReorderItemModel>> GetFilterValuesOrderAsync(int categoryId)
        {
            return await _context.FilterValues
                .Where(f => f.CategoryId == categoryId)
                .OrderBy(f => f.ViewOrder)
                .Select(f => new ReorderItemModel { Id = f.Id, ViewOrder = f.ViewOrder ?? 0 })
                .ToListAsync();
        }

        public async Task<List<ReorderItemModel>> GetFeatureHeadersOrderAsync(int categoryId)
        {
            return await _context.FeatureHeaders
                .Where(h => h.CategoryId == categoryId)
                .OrderBy(h => h.ViewOrder)
                .Select(h => new ReorderItemModel { Id = h.Id, ViewOrder = h.ViewOrder ?? 0 })
                .ToListAsync();
        }

        public async Task<List<ReorderItemModel>> GetFeaturesOrderAsync(int categoryId)
        {
            return await _context.Features
                .Where(f => f.CategoryId == categoryId)
                .OrderBy(f => f.ViewOrder)
                .Select(f => new ReorderItemModel { Id = f.Id, ViewOrder = f.ViewOrder ?? 0 })
                .ToListAsync();
        }

        public async Task<List<ReorderItemModel>> GetDiscountGroupsOrderAsync()
        {
            return await _context.DiscountGroups
                .OrderBy(g => g.ViewOrder)
                .Select(g => new ReorderItemModel { Id = g.Id, ViewOrder = g.ViewOrder ?? 0 })
                .ToListAsync();
        }

        public async Task<CategoryEntity?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}