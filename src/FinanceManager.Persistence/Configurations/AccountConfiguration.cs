using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Persistence.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
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
