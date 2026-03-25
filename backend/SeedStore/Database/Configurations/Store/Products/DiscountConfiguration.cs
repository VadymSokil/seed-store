using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.Products;

namespace SeedStore.Database.Configurations.Store.Products
{
    public class DiscountConfiguration : IEntityTypeConfiguration<DiscountEntity>
    {
        public void Configure(EntityTypeBuilder<DiscountEntity> builder)
        {
            builder.ToTable("Discounts");
            builder.HasKey(d => d.Id);
            builder.HasOne<DiscountGroupEntity>().WithMany().HasForeignKey(d => d.GroupId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<ProductEntity>().WithMany().HasForeignKey(d => d.ProductId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(d => d.DiscountPercent).IsRequired().HasColumnType("decimal(5,2)");
        }
    }
}