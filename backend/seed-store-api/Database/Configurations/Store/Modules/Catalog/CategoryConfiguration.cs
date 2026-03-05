using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Modules.Catalog;

namespace seed_store_api.Database.Configurations.Store.Modules.Catalog
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
