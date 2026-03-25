using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SeedStore.Database.Entities.Store.Catalog;
using SeedStore.Database.Entities.Store.Products;

namespace SeedStore.Database.Configurations.Store.Products
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
            builder.Property(p => p.Quantity).HasDefaultValue(0);
            builder.Property(p => p.Rating).HasColumnType("decimal(3,2)").HasDefaultValue(0);
            builder.Property(p => p.ReviewCount).HasDefaultValue(0);
            builder.Property(p => p.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(p => p.CreatedDate).IsRequired();
        }
    }
}
