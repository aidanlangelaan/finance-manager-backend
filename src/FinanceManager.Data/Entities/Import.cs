using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinanceManager.Data.Enums;

namespace FinanceManager.Data.Entities;

public class Import : AuditableEntity
{
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string OriginalFileName { get; set; }
    
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string TemporaryFileName { get; set; }
    
    [Required]
    [Column(TypeName = "tinyint")]
    public BankType BankType { get; set; }
    
    [Required]
    [Column(TypeName = "tinyint")]
    public ImportStatus Status { get; set; }

    // Relations
    public virtual IEnumerable<Transaction> Transactions { get; set; }
}