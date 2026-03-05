using Microsoft.EntityFrameworkCore;
using seed_store_api.Store.Modules.Products.Models;
using seed_store_api.Store.Modules.Products.Interfaces;
using seed_store_api.Database.Context;
using seed_store_api.Database.Entities.Store.Modules.Products;

namespace seed_store_api.Store.Modules.Products.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductSearchResponseModel>> SearchProductsAsync(string value)
        {
            return await _context.Products
                .Where(p => p.IsActive && p.Name.ToLower().Contains(value.ToLower()))
                .Select(p => new ProductSearchResponseModel
                {
                    Name = p.Name,
                    Price = p.Price,
                    ImageUrl = _context.ProductImages.Where(pi => pi.ProductId == p.Id).OrderBy(pi => pi.ViewOrder).Select(pi => pi.Url).FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task<ProductTopResponseModel> GetProductsTopAsync()
        {
            var newestTask = await _context.Products
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.CreatedDate)
                .Take(10)
                .Select(p => new ProductCardModel
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    Article = p.Article,
                    Name = p.Name,
                    Slug = p.Slug,
                    Description = p.Description,
                    Price = p.Price,
                    HasDiscount = p.HasDiscount,
                    DiscountPrice = p.DiscountPrice,
                    DiscountEndDate = p.DiscountEndDate,
                    Quantity = p.Quantity,
                    Rating = p.Rating,
                    ReviewCount = p.ReviewCount,
                    ImageUrl = _context.ProductImages.Where(pi => pi.ProductId == p.Id).OrderBy(pi => pi.ViewOrder).Select(pi => pi.Url).FirstOrDefault()
                })
                .ToListAsync();

            var popularTask = await _context.Products
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.Rating)
                .ThenByDescending(p => p.CreatedDate)
                .Take(10)
                .Select(p => new ProductCardModel
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    Article = p.Article,
                    Name = p.Name,
                    Slug = p.Slug,
                    Description = p.Description,
                    Price = p.Price,
                    HasDiscount = p.HasDiscount,
                    DiscountPrice = p.DiscountPrice,
                    DiscountEndDate = p.DiscountEndDate,
                    Quantity = p.Quantity,
                    Rating = p.Rating,
                    ReviewCount = p.ReviewCount,
                    ImageUrl = _context.ProductImages.Where(pi => pi.ProductId == p.Id).OrderBy(pi => pi.ViewOrder).Select(pi => pi.Url).FirstOrDefault()
                })
                .ToListAsync();

            var mostDiscussedTask = await _context.Products
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.ReviewCount)
                .ThenByDescending(p => p.CreatedDate)
                .Take(10)
                .Select(p => new ProductCardModel
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    Article = p.Article,
                    Name = p.Name,
                    Slug = p.Slug,
                    Description = p.Description,
                    Price = p.Price,
                    HasDiscount = p.HasDiscount,
                    DiscountPrice = p.DiscountPrice,
                    DiscountEndDate = p.DiscountEndDate,
                    Quantity = p.Quantity,
                    Rating = p.Rating,
                    ReviewCount = p.ReviewCount,
                    ImageUrl = _context.ProductImages.Where(pi => pi.ProductId == p.Id).OrderBy(pi => pi.ViewOrder).Select(pi => pi.Url).FirstOrDefault()
                })
                .ToListAsync();

            return new ProductTopResponseModel
            {
                Newest = newestTask,
                Popular = popularTask,
                MostDiscussed = mostDiscussedTask
            };
        }

        public async Task<CategoryFiltersResponseModel> GetCategoryFiltersAsync(int id)
        {
            var features = await _context.Features
                .Where(f => f.CategoryId == id)
                .OrderBy(f => f.ViewOrder)
                .Select(f => new FeatureItemModel
                {
                    Id = f.Id,
                    Name = f.Name,
                    Slug = f.Slug,
                    ViewOrder = f.ViewOrder
                })
                .ToListAsync();

            var headers = await _context.FeatureHeaders
                .Where(h => h.CategoryId == id)
                .OrderBy(h => h.ViewOrder)
                .Select(h => new FeatureHeaderItemModel
                {
                    Id = h.Id,
                    Name = h.Name,
                    ViewOrder = h.ViewOrder
                })
                .ToListAsync();

            var filterValues = await _context.FilterValues
                .Where(fv => fv.CategoryId == id && fv.IsActive)
                .OrderBy(fv => fv.ViewOrder)
                .Select(fv => new FilterValueItemModel
                {
                    Id = fv.Id,
                    FeatureId = fv.FeatureId,
                    HeaderId = fv.HeaderId,
                    Value = fv.Value,
                    ValueSlug = fv.ValueSlug,
                    ViewOrder = fv.ViewOrder
                })
                .ToListAsync();

            return new CategoryFiltersResponseModel
            {
                Features = features,
                Headers = headers,
                FilterValues = filterValues
            };
        }

        public async Task<List<ProductCardModel>> GetProductsListAsync(ProductListRequestModel request)
        {
            var query = _context.Products.Where(p => p.IsActive).AsQueryable();

            if (request.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == request.CategoryId.Value);

            if (!string.IsNullOrEmpty(request.CategorySlug))
                query = query.Where(p => _context.Categories
                    .Any(c => c.Id == p.CategoryId && c.Slug == request.CategorySlug));

            if (request.PriceFrom.HasValue)
                query = query.Where(p => p.Price >= request.PriceFrom.Value);

            if (request.PriceTo.HasValue)
                query = query.Where(p => p.Price <= request.PriceTo.Value);

            if (request.HasDiscount == true)
                query = query.Where(p => p.HasDiscount);

            if (request.InStock == true)
                query = query.Where(p => p.Quantity > 0);

            if (request.ActiveFilters != null && request.ActiveFilters.Any())
            {
                foreach (var filter in request.ActiveFilters)
                {
                    var featureSlug = filter.FeatureSlug;
                    var valueSlug = filter.ValueSlug;
                    query = query.Where(p => _context.ProductFeatures
                        .Any(pf => pf.ProductId == p.Id && pf.IsActive &&
                            _context.Features.Any(f => f.Id == pf.FeatureId && f.Slug == featureSlug) &&
                            pf.ValueSlug == valueSlug));
                }
            }

            if (request.SortByPriceAsc == true)
                query = query.OrderBy(p => p.Price);
            else if (request.SortByPriceDesc == true)
                query = query.OrderByDescending(p => p.Price);
            else
                query = query.OrderByDescending(p => p.CreatedDate);

            var skip = (request.Page - 1) * request.PageSize;

            return await query
                .Skip(skip)
                .Take(request.PageSize)
                .Select(p => new ProductCardModel
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    Article = p.Article,
                    Name = p.Name,
                    Slug = p.Slug,
                    Description = p.Description,
                    Price = p.Price,
                    HasDiscount = p.HasDiscount,
                    DiscountPrice = p.DiscountPrice,
                    DiscountEndDate = p.DiscountEndDate,
                    Quantity = p.Quantity,
                    Rating = p.Rating,
                    ReviewCount = p.ReviewCount,
                    ImageUrl = _context.ProductImages.Where(pi => pi.ProductId == p.Id).OrderBy(pi => pi.ViewOrder).Select(pi => pi.Url).FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task<ProductDetailsModel?> GetProductDetailsAsync(string idOrSlug)
        {
            ProductEntity? product;

            if (int.TryParse(idOrSlug, out int id))
                product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
            else
                product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Slug == idOrSlug && p.IsActive);

            if (product == null)
                return null;

            var imageUrls = await _context.ProductImages
                .Where(pi => pi.ProductId == product.Id)
                .OrderBy(pi => pi.ViewOrder)
                .Select(pi => pi.Url)
                .ToListAsync();

            var features = await _context.ProductFeatures
                .Where(pf => pf.ProductId == product.Id && pf.IsActive)
                .OrderBy(pf => pf.ViewOrder)
                .Select(pf => new ProductFeatureItemModel
                {
                    FeatureId = pf.FeatureId,
                    FeatureName = _context.Features
                        .Where(f => f.Id == pf.FeatureId)
                        .Select(f => f.Name)
                        .FirstOrDefault() ?? string.Empty,
                    FeatureSlug = _context.Features
                        .Where(f => f.Id == pf.FeatureId)
                        .Select(f => f.Slug)
                        .FirstOrDefault() ?? string.Empty,
                    HeaderId = pf.HeaderId,
                    HeaderName = pf.HeaderId != null ? _context.FeatureHeaders
                        .Where(h => h.Id == pf.HeaderId)
                        .Select(h => h.Name)
                        .FirstOrDefault() : null,
                    Value = pf.Value,
                    ValueSlug = pf.ValueSlug,
                    ViewOrder = pf.ViewOrder
                })
                .ToListAsync();

            return new ProductDetailsModel
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                Article = product.Article,
                Name = product.Name,
                Slug = product.Slug,
                Description = product.Description,
                Price = product.Price,
                HasDiscount = product.HasDiscount,
                DiscountPrice = product.DiscountPrice,
                DiscountEndDate = product.DiscountEndDate,
                Quantity = product.Quantity,
                Rating = product.Rating,
                ReviewCount = product.ReviewCount,
                ImageUrls = imageUrls,
                Features = features
            };
        }
    }
}
