using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        // Configure properties
        builder.Property(a => a.Name)
            .IsRequired()
            .HasColumnType("varchar(255)");

        builder.Property(j => j.Type)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(a => a.Iban)
            .HasColumnType("varchar(34)");

        builder.Property(a => a.Description)
            .HasColumnType("varchar(255)");

        builder.Property(a => a.CurrentBalance)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(a => a.IncludedInNetWorth)
            .IsRequired()
            .HasColumnType("boolean");

        builder.Property(a => a.CanTransferFrom)
            .IsRequired()
            .HasColumnType("boolean");

        builder.Property(a => a.CanTransferTo)
            .IsRequired()
            .HasColumnType("boolean");

        // Indexes
        builder.HasIndex(a => new { a.CreatedById, a.Iban });

        // Configure relationships
        builder.HasMany(a => a.SourceTransactions)
               .WithOne(t => t.SourceAccount)
               .HasForeignKey(t => t.SourceAccountId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(a => a.DestinationTransactions)
               .WithOne(t => t.DestinationAccount)
               .HasForeignKey(t => t.DestinationAccountId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.CreatedBy)
            .WithMany(u => u.Accounts)
            .HasForeignKey(a => a.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
