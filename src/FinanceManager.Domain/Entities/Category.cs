namespace FinanceManager.Domain.Entities;

public class Category : AuditableEntity
{
    public required string Name { get; set; }

    public int? ParentCategoryId { get; set; }

    // Foreign keys
    public Category? ParentCategory { get; set; }

    // Relationships
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Category> Subcategories { get; set; } = new List<Category>();
}

