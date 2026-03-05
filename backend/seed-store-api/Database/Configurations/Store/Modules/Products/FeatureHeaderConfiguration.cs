using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using seed_store_api.Database.Entities.Store.Modules.Catalog;
using seed_store_api.Database.Entities.Store.Modules.Products;

namespace seed_store_api.Database.Configurations.Store.Modules.Products
{
    public class FeatureHeaderConfiguration : IEntityTypeConfiguration<FeatureHeaderEntity>
    {
        public void Configure(EntityTypeBuilder<FeatureHeaderEntity> builder)
        {
            builder.ToTable("FeatureHeaders");

            builder.HasKey(fh => fh.Id);

            builder.HasOne<CategoryEntity>().WithMany().HasForeignKey(fh => fh.CategoryId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(fh => fh.Name).IsRequired().HasMaxLength(100);
        }
    }
}
