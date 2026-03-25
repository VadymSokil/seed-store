using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Admin.Account;

namespace SeedStore.Database.Configurations.Admin.Account
{
    public class EmployeeRefreshTokenConfiguration : IEntityTypeConfiguration<EmployeeRefreshTokenEntity>
    {
        public void Configure(EntityTypeBuilder<EmployeeRefreshTokenEntity> builder)
        {
            builder.ToTable("EmployeeRefreshTokens");
            builder.HasKey(t => t.Id);
            builder.HasOne<EmployeeEntity>().WithMany().HasForeignKey(t => t.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(t => t.Token).IsRequired().HasMaxLength(500);
            builder.Property(t => t.ExpiresAt).IsRequired();
        }
    }
}