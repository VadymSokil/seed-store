using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Entities.Store.Modules.Catalog;
using seed_store_api.Database.Entities.Store.Modules.Products;

namespace seed_store_api.Database.Configurations.Store.Modules.Products
{
    public class FilterValueConfiguration : IEntityTypeConfiguration<FilterValueEntity>
    {
        public void Configure(EntityTypeBuilder<FilterValueEntity> builder)
        {
            builder.ToTable("FilterValues");

            builder.HasKey(fv => fv.Id);

            builder.HasOne<CategoryEntity>().WithMany().HasForeignKey(fv => fv.CategoryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(fv => fv.Feature).WithMany().HasForeignKey(fv => fv.FeatureId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(fv => fv.Header).WithMany().HasForeignKey(fv => fv.HeaderId).IsRequired(false).OnDelete(DeleteBehavior.Restrict);

            builder.Property(fv => fv.Value).IsRequired().HasMaxLength(200);
            builder.Property(fv => fv.ValueSlug).IsRequired().HasMaxLength(400);
            builder.Property(fv => fv.IsActive).IsRequired().HasDefaultValue(true);
        }
    }
}
