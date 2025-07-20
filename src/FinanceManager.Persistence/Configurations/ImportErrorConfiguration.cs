using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceManager.Persistence.Configurations;

public class ImportErrorConfiguration : IEntityTypeConfiguration<ImportError>
{
    public void Configure(EntityTypeBuilder<ImportError> builder)
    {
        // Configure properties
        builder.Property(e => e.RowNumber)
            .IsRequired();

        builder.Property(e => e.RawData)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(e => e.ErrorMessage)
            .IsRequired()
            .HasColumnType("text");

        builder.Property(e => e.CreatedOnAt)
            .IsRequired()
            .HasColumnType("timestamp without time zone");

        // Configure relationships
        builder.HasOne<ImportJob>()
            .WithMany(i => i.Errors)
            .HasForeignKey(e => e.ImportJobId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
