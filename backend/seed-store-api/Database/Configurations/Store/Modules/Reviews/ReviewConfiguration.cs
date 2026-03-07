using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Modules.Account;
using seed_store_api.Database.Entities.Store.Modules.Products;
using seed_store_api.Database.Entities.Store.Modules.Reviews;

namespace seed_store_api.Database.Configurations.Store.Modules.Reviews
{
    public class ReviewConfiguration : IEntityTypeConfiguration<ReviewEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewEntity> builder)
        {
            builder.ToTable("Reviews");

            builder.HasKey(r => r.Id);

            builder.HasOne<ProductEntity>().WithMany().HasForeignKey(r => r.ProductId).IsRequired(false).OnDelete(DeleteBehavior.SetNull);
            builder.HasOne<AccountEntity>().WithMany().HasForeignKey(r => r.AccountId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(r => r.ProductNameSnapshot).IsRequired().HasMaxLength(200);
            builder.Property(r => r.ProductImageUrlSnapshot).IsRequired().HasMaxLength(500);
            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.Text).HasMaxLength(2000);
            builder.Property(r => r.CreatedAt).IsRequired();
            builder.Property(r => r.IsApproved).IsRequired().HasDefaultValue(false);
            builder.Property(r => r.ModeratorComment).HasMaxLength(500);
        }
    }
}