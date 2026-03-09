using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Modules.Orders;

namespace seed_store_api.Database.Configurations.Store.Modules.Orders
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderEntity>
    {
        public void Configure(EntityTypeBuilder<OrderEntity> builder)
        {
            builder.ToTable("Orders");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.OrderNumber).IsRequired().HasMaxLength(50);
            builder.Property(o => o.OrderDate).IsRequired();
            builder.Property(o => o.StatusCode).IsRequired().HasMaxLength(50);
            builder.Property(o => o.Comment).HasMaxLength(500);
            builder.Property(o => o.DeliveryCode).IsRequired().HasMaxLength(50);
            builder.Property(o => o.PaymentCode).IsRequired().HasMaxLength(50);
            builder.Property(o => o.PostalOfficeNumber).HasMaxLength(100);
            builder.Property(o => o.TrackingNumber).HasMaxLength(100);
            builder.Property(o => o.TotalAmount).IsRequired().HasColumnType("numeric(10,2)");
            builder.Property(o => o.AccountId);
            builder.Property(o => o.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(o => o.LastName).IsRequired().HasMaxLength(100);
            builder.Property(o => o.MiddleName).HasMaxLength(100);
            builder.Property(o => o.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(o => o.Region).HasMaxLength(100);
            builder.Property(o => o.District).HasMaxLength(100);
            builder.Property(o => o.City).HasMaxLength(100);
            builder.Property(o => o.Settlement).HasMaxLength(100);
            builder.Property(o => o.Street).HasMaxLength(200);
            builder.Property(o => o.HouseNumber).HasMaxLength(20);
            builder.Property(o => o.ApartmentNumber).HasMaxLength(20);
            builder.Property(o => o.PaidAt);

            builder.HasOne(o => o.Account).WithMany().HasForeignKey(o => o.AccountId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(o => o.Status).WithMany(s => s.Orders).HasForeignKey(o => o.StatusCode).HasPrincipalKey(s => s.Code).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.Delivery).WithMany().HasForeignKey(o => o.DeliveryCode).HasPrincipalKey(d => d.Code).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(o => o.Payment).WithMany().HasForeignKey(o => o.PaymentCode).HasPrincipalKey(p => p.Code).OnDelete(DeleteBehavior.Restrict);
        }
    }
}