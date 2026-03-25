using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SeedStore.Database.Entities.Store.Products;

namespace SeedStore.Database.Configurations.Store.Products
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImageEntity>
    {
        public void Configure(EntityTypeBuilder<ProductImageEntity> builder)
        {
            builder.ToTable("ProductImages");
            builder.HasKey(pi => pi.Id);
            builder.HasOne<ProductEntity>().WithMany().HasForeignKey(pi => pi.ProductId).OnDelete(DeleteBehavior.Cascade).IsRequired();
            builder.Property(pi => pi.Url).IsRequired().HasMaxLength(500);
            builder.Property(pi => pi.ViewOrder);
        }
    }
}
