using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SeedStore.Database.Entities.Store.Catalog;
using SeedStore.Database.Entities.Store.Products;

namespace SeedStore.Database.Configurations.Store.Products
{
    public class FeatureConfiguration : IEntityTypeConfiguration<FeatureEntity>
    {
        public void Configure(EntityTypeBuilder<FeatureEntity> builder)
        {
            builder.ToTable("Features");
            builder.HasKey(f => f.Id);
            builder.HasOne<CategoryEntity>().WithMany().HasForeignKey(f => f.CategoryId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(f => f.Name).IsRequired().HasMaxLength(100);
            builder.Property(f => f.Slug).IsRequired().HasMaxLength(200);
            builder.Property(f => f.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(f => f.ViewOrder);
        }
    }
}
