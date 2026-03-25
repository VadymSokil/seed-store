using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.StoreInfo;

namespace SeedStore.Database.Configurations.Store.StoreInfo
{
    public class DeliveryVariantConfiguration : IEntityTypeConfiguration<DeliveryVariantEntity>
    {
        public void Configure(EntityTypeBuilder<DeliveryVariantEntity> builder)
        {
            builder.ToTable("DeliveryVariants");
            builder.HasKey(dv => dv.Id);
            builder.Property(dv => dv.Code).IsRequired().HasMaxLength(50);
            builder.Property(dv => dv.Name).IsRequired().HasMaxLength(100);
            builder.Property(dv => dv.Description).IsRequired().HasMaxLength(1000);
            builder.Property(dv => dv.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(dv => dv.ViewOrder);
        }
    }
}
