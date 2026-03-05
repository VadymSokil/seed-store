using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Entities.Store.Modules.Catalog;
using seed_store_api.Database.Entities.Store.Modules.Products;

namespace seed_store_api.Database.Configurations.Store.Modules.Products
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
        }
    }
}
