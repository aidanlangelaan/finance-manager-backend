using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Domain.Entities;

public class Transaction : AuditableEntity
{
    [Required]
    [Column(TypeName = "int")]
    public int SourceAccountId { get; set; }
    
    [Required]
    [Column(TypeName = "int")]
    public int DestinationAccountId { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    
    [Required]
    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }
    
    [Column(TypeName = "varchar(255)")]
    public string? Description { get; set; }
    
    [Column(TypeName = "int")]
    public int? CategoryId { get; set; }

    // Foreign keys
    [ForeignKey("SourceAccountId")]
    public Account SourceAccount { get;set; }
    
    [ForeignKey("DestinationAccountId")]
    public Account DestinationAccount { get;set; }
    
    [ForeignKey("CategoryId")]
    public Category Category { get;set; }
    
    // Relationships
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();
}