using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Modules.Account;

namespace seed_store_api.Database.Configurations.Store.Modules.Account
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
        {
            builder.ToTable("RefreshTokens");

            builder.HasKey(r => r.Id);

            builder.HasOne<AccountEntity>().WithMany().HasForeignKey(r => r.AccountId).OnDelete(DeleteBehavior.Cascade);

            builder.Property(r => r.Token).IsRequired();
            builder.HasIndex(r => r.Token).IsUnique();
            builder.Property(r => r.ExpiresAt).IsRequired();
            builder.Property(r => r.CreatedAt).IsRequired();
        }
    }
}