using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Admin.Account;

namespace SeedStore.Database.Configurations.Admin.Account
{
    public class EmployeeActivityConfiguration : IEntityTypeConfiguration<EmployeeActivityEntity>
    {
        public void Configure(EntityTypeBuilder<EmployeeActivityEntity> builder)
        {
            builder.ToTable("EmployeeActivity");
            builder.HasKey(e => e.Id);
            builder.HasOne<EmployeeEntity>().WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(e => e.Action).IsRequired().HasMaxLength(500);
            builder.Property(e => e.Before);
            builder.Property(e => e.After);
            builder.Property(e => e.CreatedAt).IsRequired();
        }
    }
}