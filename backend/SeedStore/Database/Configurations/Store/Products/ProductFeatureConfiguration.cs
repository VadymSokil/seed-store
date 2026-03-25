using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SeedStore.Database.Entities.Store.Products;

namespace SeedStore.Database.Configurations.Store.Products
{
    public class ProductFeatureConfiguration : IEntityTypeConfiguration<ProductFeatureEntity>
    {
        public void Configure(EntityTypeBuilder<ProductFeatureEntity> builder)
        {
            builder.ToTable("ProductFeatures");
            builder.HasKey(pf => pf.Id);
            builder.HasOne<ProductEntity>().WithMany().HasForeignKey(pf => pf.ProductId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            builder.HasOne(pf => pf.Feature).WithMany().HasForeignKey(pf => pf.FeatureId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(pf => pf.Header).WithMany().HasForeignKey(pf => pf.HeaderId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
            builder.Property(pf => pf.Value).IsRequired().HasMaxLength(200);
            builder.Property(pf => pf.ValueSlug).IsRequired().HasMaxLength(400);
            builder.Property(pf => pf.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(pf => pf.ViewOrder);
        }
    }
}
