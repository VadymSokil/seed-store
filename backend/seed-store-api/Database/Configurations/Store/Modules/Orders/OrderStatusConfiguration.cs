using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Modules.Orders;

namespace seed_store_api.Database.Configurations.Store.Modules.Orders
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