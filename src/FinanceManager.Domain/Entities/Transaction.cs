namespace FinanceManager.Domain.Entities;

public class Transaction : AuditableEntity
{
    public required int SourceAccountId { get; set; }

    public required int DestinationAccountId { get; set; }

    public required decimal Amount { get; set; }

    public required DateTime Date { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    // Foreign keys
    public Account SourceAccount { get; set; } = null!;

    public Account DestinationAccount { get;set; } = null!;

    public Category Category { get;set; } = null!;

    // Relationships
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
