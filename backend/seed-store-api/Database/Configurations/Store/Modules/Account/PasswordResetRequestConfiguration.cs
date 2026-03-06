using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Modules.Account;

namespace seed_store_api.Database.Configurations.Store.Modules.Account
{
    public class PasswordResetRequestConfiguration : IEntityTypeConfiguration<PasswordResetRequestEntity>
    {
        public void Configure(EntityTypeBuilder<PasswordResetRequestEntity> builder)
        {
            builder.ToTable("PasswordResetRequests");

            builder.HasKey(p => p.Id);

            builder.HasOne<AccountEntity>().WithMany().HasForeignKey(p => p.AccountId).OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(p => p.AccountId).IsUnique();
            builder.Property(p => p.Code).IsRequired().HasMaxLength(10);
            builder.Property(p => p.Token).IsRequired();
            builder.Property(p => p.ExpiresAt).IsRequired();
        }
    }
}