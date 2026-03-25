using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.Orders;

namespace SeedStore.Database.Configurations.Store.Orders
{
    public class OrderStatusConfiguration : IEntityTypeConfiguration<OrderStatusEntity>
    {
        public void Configure(EntityTypeBuilder<OrderStatusEntity> builder)
        {
            builder.ToTable("OrderStatuses");
            builder.HasKey(os => os.Id);
            builder.Property(os => os.Code).IsRequired().HasMaxLength(50);
            builder.Property(os => os.Name).IsRequired().HasMaxLength(100);
            builder.Property(os => os.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(os => os.ViewOrder);
            builder.HasIndex(os => os.Code).IsUnique();
        }
    }
}