using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.Catalog;

namespace SeedStore.Database.Configurations.Store.Catalog
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(c => c.Id);
            builder.HasOne<CategoryEntity>().WithMany().HasForeignKey(c => c.ParentId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Slug).IsRequired().HasMaxLength(200);
            builder.Property(c => c.ImageUrl).HasMaxLength(500);
            builder.Property(c => c.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(c => c.ViewOrder);
        }
    }
}
