using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceManager.Domain.Entities;

public class Category : AuditableEntity
{
    [Required]
    [Column(TypeName = "varchar(100)")]
    public required string Name { get; set; }

    [Column(TypeName = "int")]
    public int? ParentCategoryId { get; set; }

    // Foreign keys
    [ForeignKey("ParentCategoryId")]
    public Category? ParentCategory { get; set; }

    // Relationships
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Category> Subcategories { get; set; } = new List<Category>();
}
