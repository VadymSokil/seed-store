using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using seed_store_api.Database.Entities.Store.Support.Logging;

namespace seed_store_api.Database.Configurations.Store.Support.Logging
{
    public class LoggingConfiguration : IEntityTypeConfiguration<LoggingEntity>
    {
        public void Configure(EntityTypeBuilder<LoggingEntity> builder)
        {
            builder.ToTable("Logs");
            builder.HasKey(l => l.Id);
            builder.Property(l => l.LogId).IsRequired();
            builder.HasIndex(l => l.LogId).IsUnique();
            builder.Property(l => l.LogTime).IsRequired();
            builder.Property(l => l.Status).IsRequired().HasMaxLength(10);
            builder.Property(l => l.Address).IsRequired().HasMaxLength(200);
            builder.Property(l => l.Input);
            builder.Property(l => l.Output);
            builder.Property(l => l.Message).HasMaxLength(2000);
            builder.Property(l => l.ExecutionTime).IsRequired();
        }
    }
}