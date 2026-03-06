using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Modules.Account;

namespace seed_store_api.Database.Configurations.Store.Modules.Account
{
    public class AccountConfiguration : IEntityTypeConfiguration<AccountEntity>
    {
        public void Configure(EntityTypeBuilder<AccountEntity> builder)
        {
            builder.ToTable("Accounts");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Email).IsRequired().HasMaxLength(254);
            builder.HasIndex(a => a.Email).IsUnique();
            builder.Property(a => a.PasswordHash).IsRequired();
            builder.Property(a => a.PhoneNumber).HasMaxLength(20);
            builder.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(a => a.LastName).IsRequired().HasMaxLength(100);
            builder.Property(a => a.MiddleName).HasMaxLength(100);
            builder.Property(a => a.CreatedAt).IsRequired();
        }
    }
}