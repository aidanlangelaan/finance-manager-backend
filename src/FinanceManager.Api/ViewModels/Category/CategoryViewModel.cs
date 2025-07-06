namespace FinanceManager.Api.ViewModels.Category;

public class CategoryViewModel
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int? ParentCategoryId { get; set; }
}
