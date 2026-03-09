using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Modules.Orders;

namespace seed_store_api.Database.Configurations.Store.Modules.Orders
{
    public class OrderTransactionConfiguration : IEntityTypeConfiguration<OrderTransactionEntity>
    {
        public void Configure(EntityTypeBuilder<OrderTransactionEntity> builder)
        {
            builder.ToTable("OrderTransactions");
            builder.HasKey(ot => ot.Id);
            builder.Property(ot => ot.OrderId).IsRequired();
            builder.Property(ot => ot.LiqPayPaymentId).IsRequired();
            builder.Property(ot => ot.LiqPayOrderId).IsRequired().HasMaxLength(100);
            builder.Property(ot => ot.Status).IsRequired().HasMaxLength(50);
            builder.Property(ot => ot.Amount).IsRequired().HasColumnType("numeric(10,2)");
            builder.Property(ot => ot.Currency).IsRequired().HasMaxLength(10);
            builder.Property(ot => ot.ErrCode).HasMaxLength(50);
            builder.Property(ot => ot.ErrDescription).HasMaxLength(500);
            builder.Property(ot => ot.CreatedAt).IsRequired();

            builder.HasOne(ot => ot.Order).WithMany(o => o.Transactions).HasForeignKey(ot => ot.OrderId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}