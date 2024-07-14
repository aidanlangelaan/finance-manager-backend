namespace FinanceManager.Business.Services.Models;

public class UpdateCategoryDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}