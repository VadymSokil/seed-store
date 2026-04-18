using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.StoreInfo;

namespace SeedStore.Database.Configurations.Store.StoreInfo
{
    public class ContactsConfiguration : IEntityTypeConfiguration<ContactsEntity>
    {
        public void Configure(EntityTypeBuilder<ContactsEntity> builder)
        {
            builder.ToTable("Contacts");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Content).IsRequired();
            builder.HasData(new ContactsEntity { Id = 1, Content = string.Empty });
        }
    }
}