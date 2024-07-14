namespace FinanceManager.Api.Models;

public class CreateCategoryViewModel
{
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
}