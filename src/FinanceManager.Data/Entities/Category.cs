using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Data.Entities;

public class Category : AuditableEntity
{
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string? Name { get; set; }
    
    [Column(TypeName = "varchar(255)")]
    public string? Description { get; set; }

    // Relations
    public virtual IEnumerable<Transaction> Transactions { get; set; } = default!;
}