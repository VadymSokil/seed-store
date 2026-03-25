using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeedStore.Database.Entities.Admin.Roles;

namespace SeedStore.Database.Configurations.Admin.Roles
{
    public class RoleConfiguration : IEntityTypeConfiguration<RoleEntity>
    {
        public void Configure(EntityTypeBuilder<RoleEntity> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(r => r.Code).IsUnique();
            builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
            builder.Property(r => r.Priority).IsRequired().HasDefaultValue(99);
            builder.Property(r => r.IsActive).IsRequired().HasDefaultValue(true);
            builder.Property(r => r.ViewOrder);
        }
    }
}