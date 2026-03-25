using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Admin.Account;
using SeedStore.Database.Entities.Store.Products;
using SeedStore.Database.Entities.Store.Reviews;

namespace SeedStore.Database.Configurations.Store.Reviews
{
    public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewEntity> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(r => r.Id);
            builder.HasOne<ProductEntity>().WithMany().HasForeignKey(r => r.ProductId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne(r => r.Account).WithMany().HasForeignKey(r => r.AccountId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(r => r.ProductNameSnapshot).IsRequired().HasMaxLength(200);
            builder.Property(r => r.ProductImageUrlSnapshot).IsRequired().HasMaxLength(500);
            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.Text).HasMaxLength(2000);
            builder.Property(r => r.CreatedAt).IsRequired();
            builder.HasOne<ReviewStatusEntity>().WithMany().HasForeignKey(r => r.StatusCode).HasPrincipalKey(s => s.Code).OnDelete(DeleteBehavior.Restrict);
            builder.Property(r => r.StatusCode).IsRequired().HasMaxLength(50).HasDefaultValue("pending");
            builder.Property(r => r.ModeratorComment).HasMaxLength(500);
            builder.HasOne<EmployeeEntity>().WithMany().HasForeignKey(r => r.TakenByEmployeeId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
        }
    }
}