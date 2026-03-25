using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Admin.Roles;

namespace SeedStore.Database.Configurations.Admin.Roles
{
    public class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.ToTable("Permissions");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Code).IsRequired().HasMaxLength(100);
            builder.HasIndex(p => p.Code).IsUnique();
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.Property(p => p.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(p => p.ViewOrder);
        }
    }
}