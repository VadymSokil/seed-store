using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Modules.Reviews;

namespace seed_store_api.Database.Configurations.Store.Modules.Reviews
{
    public class ReviewReplyConfiguration : IEntityTypeConfiguration<ReviewReplyEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewReplyEntity> builder)
        {
            builder.ToTable("ReviewReplies");

            builder.HasKey(r => r.Id);

            builder.HasOne<ReviewEntity>().WithMany().HasForeignKey(r => r.ReviewId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(r => r.Text).IsRequired().HasMaxLength(2000);
            builder.Property(r => r.CreatedAt).IsRequired();
        }
    }
}