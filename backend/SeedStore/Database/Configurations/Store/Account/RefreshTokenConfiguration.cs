using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.Account;

namespace SeedStore.Database.Configurations.Store.Account
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.ToTable("RefreshTokens");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Token).IsRequired();
            builder.HasIndex(r => r.Token).IsUnique();
            builder.Property(r => r.RememberMe).IsRequired().HasDefaultValue(false);
            builder.Property(r => r.ExpiresAt).IsRequired();
            builder.Property(r => r.CreatedAt).IsRequired();
            builder.HasOne<AccountEntity>().WithMany().HasForeignKey(r => r.AccountId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}