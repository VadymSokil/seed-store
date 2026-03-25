using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Admin.Orders;

namespace SeedStore.Database.Configurations.Admin.Orders
{
    public class OrderActionConfiguration : IEntityTypeConfiguration<OrderActionEntity>
    {
        public void Configure(EntityTypeBuilder<OrderActionEntity> builder)
        {
            builder.ToTable("OrderActions");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Name).IsRequired().HasMaxLength(100);
            builder.Property(o => o.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(o => o.Code).IsUnique();
            builder.Property(os => os.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(os => os.ViewOrder);
        }
    }
}