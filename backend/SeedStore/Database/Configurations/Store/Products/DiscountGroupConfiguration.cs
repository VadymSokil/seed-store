using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.Products;

namespace SeedStore.Database.Configurations.Store.Products
{
    public class DiscountGroupConfiguration : IEntityTypeConfiguration<DiscountGroupEntity>
    {
        public void Configure(EntityTypeBuilder<DiscountGroupEntity> builder)
        {
            builder.ToTable("DiscountGroups");
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Name).IsRequired().HasMaxLength(200);
            builder.Property(d => d.StartDate).IsRequired();
            builder.Property(d => d.EndDate).IsRequired();
            builder.Property(d => d.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(d => d.ViewOrder);
        }
    }
}