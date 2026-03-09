using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Modules.Orders;

namespace seed_store_api.Database.Configurations.Store.Modules.Orders
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItemEntity>
    {
        public void Configure(EntityTypeBuilder<OrderItemEntity> builder)
        {
            builder.ToTable("OrderItems");
            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.OrderId).IsRequired();
            builder.Property(oi => oi.ProductId).IsRequired();
            builder.Property(oi => oi.ProductNameSnapshot).IsRequired().HasMaxLength(200);
            builder.Property(oi => oi.ProductImageUrlSnapshot).HasMaxLength(500);
            builder.Property(oi => oi.Quantity).IsRequired();
            builder.Property(oi => oi.Price).IsRequired().HasColumnType("numeric(10,2)");

            builder.HasOne(oi => oi.Order).WithMany(o => o.Items).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(oi => oi.Product).WithMany().HasForeignKey(oi => oi.ProductId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}