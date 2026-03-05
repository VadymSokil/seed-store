using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Configurations.Store.Modules.Catalog;
using seed_store_api.Database.Entities.Store.Modules.Catalog;
using seed_store_api.Database.Entities.Store.Modules.Products;
using seed_store_api.Database.Entities.Store.Modules.StoreInfo;

namespace seed_store_api.Database.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CategoryEntity> Categories { get; set; }

        public DbSet<DeliveryVariantEntity> DeliveryVariants { get; set; }
        public DbSet<PaymentVariantEntity> PaymentVariants { get; set; }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductImageEntity> ProductImages { get; set; }
        public DbSet<FeatureEntity> Features { get; set; }
        public DbSet<FeatureHeaderEntity> FeatureHeaders { get; set; }
        public DbSet<FilterValueEntity> FilterValues { get; set; }
        public DbSet<ProductFeatureEntity> ProductFeatures { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}
