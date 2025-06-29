using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinanceManager.Domain.Enums;

namespace FinanceManager.Domain.Entities;

public class Account : AuditableEntity
{
    [Required]
    [Column(TypeName = "varchar(255)")]
    public required string Name { get; set; }

    [Required]
    [Column(TypeName = "smallint")]
    public required AccountType Type { get; set; }

    [Column(TypeName = "varchar(255)")]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public required decimal CurrentBalance { get; set; }

    [Required]
    [Column(TypeName = "boolean")]
    public bool IncludedInNetWorth { get; set; } = true;

    [Required]
    [Column(TypeName = "boolean")]
    public bool CanTransferFrom { get; set; } = true;

    [Required]
    [Column(TypeName = "boolean")]
    public bool CanTransferTo { get; set; } = true;

    // Relationships
    public ICollection<Transaction> SourceTransactions { get; set; } = new List<Transaction>();
    public ICollection<Transaction> DestinationTransactions { get; set; } = new List<Transaction>();
}
