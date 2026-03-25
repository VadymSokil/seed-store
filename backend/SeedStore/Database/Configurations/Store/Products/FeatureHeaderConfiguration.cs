using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SeedStore.Database.Entities.Store.Catalog;
using SeedStore.Database.Entities.Store.Products;

namespace SeedStore.Database.Configurations.Store.Products
{
    public class FeatureHeaderConfiguration : IEntityTypeConfiguration<FeatureHeaderEntity>
    {
        public void Configure(EntityTypeBuilder<FeatureHeaderEntity> builder)
        {
            builder.ToTable("FeatureHeaders");
            builder.HasKey(fh => fh.Id);
            builder.HasOne<CategoryEntity>().WithMany().HasForeignKey(fh => fh.CategoryId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(fh => fh.Name).IsRequired().HasMaxLength(100);
            builder.Property(fh => fh.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(fh => fh.ViewOrder);
        }
    }
}
