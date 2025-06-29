using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Persistence.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasMany(t => t.Tags)
               .WithMany(t => t.Transactions)
               .UsingEntity(j => j.ToTable("TransactionTags"));

        builder.HasOne(t => t.CreatedBy)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
