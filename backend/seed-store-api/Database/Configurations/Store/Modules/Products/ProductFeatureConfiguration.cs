using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Entities.Store.Modules.Products;

namespace seed_store_api.Database.Configurations.Store.Modules.Products
{
    public class ProductFeatureConfiguration : IEntityTypeConfiguration<ProductFeatureEntity>
    {
        public void Configure(EntityTypeBuilder<ProductFeatureEntity> builder)
        {
            builder.ToTable("ProductFeatures");

            builder.HasKey(pf => pf.Id);

            builder.HasOne<ProductEntity>().WithMany().HasForeignKey(pf => pf.ProductId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            builder.HasOne<FeatureEntity>().WithMany().HasForeignKey(pf => pf.FeatureId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne<FeatureHeaderEntity>().WithMany().HasForeignKey(pf => pf.HeaderId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);

            builder.Property(pf => pf.Value).IsRequired().HasMaxLength(200);
            builder.Property(pf => pf.ValueSlug).IsRequired().HasMaxLength(400);
            builder.Property(pf => pf.IsActive).IsRequired().HasDefaultValue(true);
        }
    }
}
