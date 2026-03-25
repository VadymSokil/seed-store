using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.StoreInfo;

namespace SeedStore.Database.Configurations.Store.StoreInfo
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
            builder.Property(pv => pv.ViewOrder);
        }
    }
}
