using FinanceManager.Domain.Enums;

namespace FinanceManager.Domain.Entities;

public class Account : AuditableEntity
{
    public required string Name { get; set; }

    public required AccountType Type { get; set; }

    public string? Iban { get; set; }

    public string? Description { get; set; }

    public required decimal CurrentBalance { get; set; }

    public bool IncludedInNetWorth { get; set; } = true;

    public bool CanTransferFrom { get; set; } = true;

    public bool CanTransferTo { get; set; } = true;

    // Relationships
    public ICollection<Transaction> SourceTransactions { get; set; } = new List<Transaction>();
    public ICollection<Transaction> DestinationTransactions { get; set; } = new List<Transaction>();
}
