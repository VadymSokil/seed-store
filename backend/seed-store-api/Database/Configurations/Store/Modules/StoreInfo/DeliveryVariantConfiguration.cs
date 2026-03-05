using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Modules.StoreInfo;

namespace seed_store_api.Database.Configurations.Store.Modules.StoreInfo
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
        }
    }
}
