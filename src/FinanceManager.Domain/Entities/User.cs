using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Domain.Entities;

public class User : AuditableEntity
{
    [Required]
    [Column(TypeName = "uuid")]
    public Guid KeycloakId { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(100)")]
    public string DisplayName { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(100)")]
    public string Email { get; set; }

    // Relationships
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
