namespace FinanceManager.Api.ViewModels.Category;

public class CreateCategoryViewModel
{
    public required string Name { get; set; }

    public int? ParentCategoryId { get; set; }
}
