using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Admin.Account;

namespace SeedStore.Database.Configurations.Admin.Account
{
    public class BlockedEmployeeConfiguration : IEntityTypeConfiguration<BlockedEmployeeEntity>
    {
        public void Configure(EntityTypeBuilder<BlockedEmployeeEntity> builder)
        {
            builder.ToTable("BlockedEmployees");
            builder.HasKey(b => b.Id);
            builder.HasOne<EmployeeEntity>().WithMany().HasForeignKey(b => b.TargetEmployeeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne<EmployeeEntity>().WithMany().HasForeignKey(b => b.BlockedByEmployeeId).OnDelete(DeleteBehavior.Restrict);
            builder.Property(b => b.Reason).IsRequired().HasMaxLength(500);
            builder.Property(b => b.CreatedAt).IsRequired();
        }
    }
}