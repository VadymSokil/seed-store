using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.Reviews;

namespace SeedStore.Database.Configurations.Store.Reviews
{
    public class ReviewReplyConfiguration : IEntityTypeConfiguration<ReviewReplyEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewReplyEntity> builder)
        {
            builder.ToTable("ReviewReplies");
            builder.HasKey(r => r.Id);
            builder.HasOne(r => r.Review).WithMany().HasForeignKey(r => r.ReviewId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(r => r.Text).IsRequired().HasMaxLength(2000);
            builder.Property(r => r.CreatedAt).IsRequired();
        }
    }
}