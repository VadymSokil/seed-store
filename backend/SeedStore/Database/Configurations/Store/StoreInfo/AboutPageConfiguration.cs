using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.StoreInfo;

namespace SeedStore.Database.Configurations.Store.StoreInfo
{
    public class AboutPageConfiguration : IEntityTypeConfiguration<AboutPageEntity>
    {
        public void Configure(EntityTypeBuilder<AboutPageEntity> builder)
        {
            builder.ToTable("AboutPage");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Content).IsRequired();
            builder.HasData(new AboutPageEntity { Id = 1, Content = string.Empty });
        }
    }
}