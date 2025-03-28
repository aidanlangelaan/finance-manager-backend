namespace FinanceManager.Domain.Entities;

public class UserProfile : AuditableEntity
{
    public string KeycloakId { get; set; } = null!;

    public string? DisplayName { get; set; }

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
