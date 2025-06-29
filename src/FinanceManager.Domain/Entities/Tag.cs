using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Domain.Entities;

public class Tag : AuditableEntity
{
    [Required]
    [Column(TypeName = "varchar(100)")]
    public required string Name { get; set; }

    // Relationships
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
