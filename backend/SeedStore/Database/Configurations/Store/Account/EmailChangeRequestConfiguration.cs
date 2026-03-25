using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Store.Account;

namespace SeedStore.Database.Configurations.Store.Account
{
    public class EmailChangeRequestConfiguration : IEntityTypeConfiguration<EmailChangeRequestEntity>
    {
        public void Configure(EntityTypeBuilder<EmailChangeRequestEntity> builder)
        {
            builder.ToTable("EmailChangeRequests");
            builder.HasKey(e => e.Id);
            builder.HasOne<AccountEntity>().WithMany().HasForeignKey(e => e.AccountId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(e => e.AccountId).IsUnique();
            builder.Property(e => e.NewEmail).IsRequired().HasMaxLength(254);
            builder.Property(e => e.Code).IsRequired().HasMaxLength(10);
            builder.Property(e => e.ExpiresAt).IsRequired();
        }
    }
}