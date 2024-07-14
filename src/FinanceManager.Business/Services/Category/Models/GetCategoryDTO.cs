namespace FinanceManager.Business.Services.Models;

public class GetCategoryDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}