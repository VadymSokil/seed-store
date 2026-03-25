using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Admin.Account;

namespace SeedStore.Database.Configurations.Admin.Account
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<EmployeeEntity>
    {
        public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
        {
            builder.ToTable("Employees");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Login).IsRequired().HasMaxLength(50);
            builder.Property(e => e.PasswordHash).IsRequired().HasMaxLength(500);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.HasOne(e => e.Role).WithMany().HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(e => e.IsBlocked).IsRequired().HasDefaultValue(false);
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.Property(e => e.IsOnShift).IsRequired().HasDefaultValue(false);
        }
    }
}