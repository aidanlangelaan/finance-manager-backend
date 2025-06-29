using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        // Configure properties
        builder.Property(t => t.SourceAccountId)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(t => t.DestinationAccountId)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(t => t.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.Date)
            .IsRequired()
            .HasColumnType("timestamp with time zone");

        builder.Property(t => t.Description)
            .HasColumnType("varchar(255)");

        builder.Property(t => t.CategoryId)
            .HasColumnType("int");

        // Configure relationships
        builder.HasMany(t => t.Tags)
               .WithMany(t => t.Transactions)
               .UsingEntity(j => j.ToTable("TransactionTags"));

        builder.HasOne(t => t.CreatedBy)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
