using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Entities.Store.Modules.Catalog;
using seed_store_api.Database.Entities.Store.Modules.Products;

namespace seed_store_api.Database.Configurations.Store.Modules.Products
{
    public class ProductConfiguration : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.HasOne<CategoryEntity>().WithMany().HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.Article).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.Slug).IsRequired().HasMaxLength(400);
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.Price).IsRequired().HasColumnType("decimal(10,2)");
            builder.Property(p => p.HasDiscount).IsRequired().HasDefaultValue(false);
            builder.Property(p => p.DiscountPrice).HasColumnType("decimal(10,2)").HasDefaultValue(0);
            builder.Property(p => p.Quantity).HasDefaultValue(0);
            builder.Property(p => p.Rating).HasColumnType("decimal(3,2)").HasDefaultValue(0);
            builder.Property(p => p.ReviewCount).HasDefaultValue(0);
            builder.Property(p => p.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(p => p.CreatedDate).IsRequired();
        }
    }
}
