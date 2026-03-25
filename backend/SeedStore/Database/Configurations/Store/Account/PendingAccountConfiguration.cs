using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.Account;

namespace SeedStore.Database.Configurations.Store.Account
{
    public class PendingAccountConfiguration : IEntityTypeConfiguration<PendingAccountEntity>
    {
        public void Configure(EntityTypeBuilder<PendingAccountEntity> builder)
        {
            builder.ToTable("PendingAccounts");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Email).IsRequired().HasMaxLength(254);
            builder.HasIndex(p => p.Email).IsUnique();
            builder.Property(p => p.PasswordHash).IsRequired();
            builder.Property(p => p.PhoneNumber).HasMaxLength(20);
            builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
            builder.Property(p => p.MiddleName).HasMaxLength(100);
            builder.Property(p => p.Code).IsRequired().HasMaxLength(10);
            builder.Property(p => p.ExpiresAt).IsRequired();
        }
    }
}