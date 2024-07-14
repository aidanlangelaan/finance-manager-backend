namespace FinanceManager.Api.Models;

public class UpdateCategoryViewModel
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}