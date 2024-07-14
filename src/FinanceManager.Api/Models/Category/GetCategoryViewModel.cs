namespace FinanceManager.Api.Models;

public class GetCategoryViewModel
{
    public int Id { get; init; }

    public string Name { get; init; } = default!;

    public string? Description { get; init; }
}