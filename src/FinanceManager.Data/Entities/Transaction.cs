using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FinanceManager.Data.Constants;

namespace FinanceManager.Data.Entities;

public class Transaction : AuditableEntity
{
    [Required]
    [Column(TypeName = "float")]
    public double Amount { get; set; }

    [Required]
    [Column(TypeName = "int")]
    public int FromAccountId { get; set; }

    [Required]
    [Column(TypeName = "int")]
    public int ToAccountId { get; set; }

    [Required]
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Column(TypeName = "varchar(255)")]
    public string? Description { get; set; }

    [Required]
    [Column(TypeName = "int")]
    public int CategoryId { get; set; } = CategoryConstants.UncategorizedId;
    
    [Column(TypeName = "int")]
    public int? ImportId { get; set; }

    private object[] HashProperties => [Amount, FromAccountId, ToAccountId, Date, Description ?? string.Empty, CategoryId];
    
    // Foreign keys
    [ForeignKey("FromAccountId")]
    public Account? FromAccount { get; set; }
    
    [ForeignKey("ToAccountId")]
    public Account? ToAccount { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category? Category { get; set; }
    
    // Relations
    public virtual Import? Import { get; set; }
}