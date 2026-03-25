using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Admin.Orders;
using SeedStore.Database.Entities.Store.Orders;

namespace SeedStore.Database.Configurations.Admin.Orders
{
    public class OrderProcessingConfiguration : IEntityTypeConfiguration<OrderProcessingEntity>
    {
        public void Configure(EntityTypeBuilder<OrderProcessingEntity> builder)
        {
            builder.ToTable("OrderProcessings");
            builder.HasKey(o => o.Id);
            builder.HasOne<OrderEntity>().WithMany().HasForeignKey(o => o.OrderId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<EmployeeEntity>().WithMany().HasForeignKey(o => o.EmployeeId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(o => o.ActionCode).IsRequired().HasMaxLength(50);
            builder.Property(o => o.CreatedAt).IsRequired();
        }
    }
}