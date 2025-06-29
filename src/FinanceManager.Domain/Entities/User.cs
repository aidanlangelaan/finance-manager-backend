namespace FinanceManager.Domain.Entities;

public class User : AuditableEntity
{
    public required Guid KeycloakId { get; set; }

    public required string DisplayName { get; set; }

    public required string Email { get; set; }

    // Relationships
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public ICollection<User> Users { get; set; } = new List<User>();
}
