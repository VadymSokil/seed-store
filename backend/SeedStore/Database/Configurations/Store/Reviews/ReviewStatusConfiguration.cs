using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.Reviews;

namespace SeedStore.Database.Configurations.Store.Reviews
{
    public class ReviewStatusConfiguration : IEntityTypeConfiguration<ReviewStatusEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewStatusEntity> builder)
        {
            builder.ToTable("ReviewStatuses");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(r => r.Code).IsUnique();
            builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
            builder.Property(r => r.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(r => r.ViewOrder);
        }
    }
}