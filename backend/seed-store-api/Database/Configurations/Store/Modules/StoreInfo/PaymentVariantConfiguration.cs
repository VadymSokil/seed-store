using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Modules.StoreInfo;

namespace seed_store_api.Database.Configurations.Store.Modules.StoreInfo
{
    public class PaymentVariantConfiguration : IEntityTypeConfiguration<PaymentVariantEntity>
    {
        public void Configure(EntityTypeBuilder<PaymentVariantEntity> builder)
        {
            builder.ToTable("PaymentVariants");
            builder.HasKey(pv => pv.Id);
            builder.Property(pv => pv.Code).IsRequired().HasMaxLength(50);
            builder.Property(pv => pv.Name).IsRequired().HasMaxLength(100);
            builder.Property(pv => pv.Description).IsRequired().HasMaxLength(1000);
            builder.Property(pv => pv.IsActive).IsRequired().HasDefaultValue(true);
        }
    }
}
