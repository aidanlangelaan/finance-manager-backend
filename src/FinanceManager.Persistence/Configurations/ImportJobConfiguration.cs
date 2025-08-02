using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceManager.Persistence.Configurations;

public class ImportJobConfiguration : IEntityTypeConfiguration<ImportJob>
{
    public void Configure(EntityTypeBuilder<ImportJob> builder)
    {
        // Configure properties
        builder.Property(j => j.OriginalFileName)
            .IsRequired()
            .HasColumnType("varchar(255)");

        builder.Property(j => j.StoredFileName)
            .IsRequired()
            .HasColumnType("varchar(100)");

        builder.Property(j => j.MappingProfile)
            .IsRequired()
            .HasColumnType("jsonb");

        builder.Property(j => j.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasColumnType("varchar(50)");

        builder.Property(j => j.StartedAt)
            .HasColumnType("timestamp");

        builder.Property(j => j.FinishedAt)
            .HasColumnType("timestamp");

        builder.Property(j => j.TotalRows)
            .IsRequired();

        builder.Property(j => j.SuccessCount)
            .IsRequired();

        builder.Property(j => j.ErrorCount)
            .IsRequired();

        builder.Property(j => j.NotifyOnCompletion)
            .IsRequired()
            .HasColumnType("boolean");

        // Configure relationships
        builder.HasMany(i => i.Errors)
               .WithOne(e => e.ImportJob)
               .HasForeignKey(e => e.ImportJobId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
