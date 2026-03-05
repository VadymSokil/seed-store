using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Entities.Store.Modules.Products;

namespace seed_store_api.Database.Configurations.Store.Modules.Products
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImageEntity>
    {
        public void Configure(EntityTypeBuilder<ProductImageEntity> builder)
        {
            builder.ToTable("ProductImages");

            builder.HasKey(pi => pi.Id);

            builder.HasOne<ProductEntity>().WithMany().HasForeignKey(pi => pi.ProductId).OnDelete(DeleteBehavior.Cascade).IsRequired();

            builder.Property(pi => pi.Url).IsRequired().HasMaxLength(500);
        }
    }
}
