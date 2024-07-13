using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinanceManager.Data.Enums;

namespace FinanceManager.Data.Entities;

public class Account : AuditableEntity
{
    [Column(TypeName = "varchar(255)")]
    public string? Iban { get; set; }

    [Required]
    [Column(TypeName = "varchar(255)")]
    public string Name { get; set; }

    [Required]
    [Column(TypeName = "tinyint")]
    public AccountType Type { get; set; } = AccountType.Expense;

    // Relations
    public virtual IEnumerable<Transaction> TransactionsFrom { get; set; }
    public virtual IEnumerable<Transaction> TransactionsTo { get; set; }
}