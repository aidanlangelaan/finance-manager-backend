namespace FinanceManager.Api.ViewModels.Category;

public class UpdateCategoryViewModel
{
    public required string Name { get; set; }

    public int? ParentCategoryId { get; set; }
}
