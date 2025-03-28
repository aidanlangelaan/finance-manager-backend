namespace FinanceManager.Domain.Entities;

public class Transaction : AuditableEntity
{
    public decimal Amount { get; set; }
    public string Category { get; set; } = string.Empty;
}