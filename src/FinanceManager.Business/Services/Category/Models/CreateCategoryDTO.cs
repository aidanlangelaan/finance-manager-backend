namespace FinanceManager.Business.Services.Models;

public class CreateCategoryDTO
{
    public string Name { get; set; } = default!;
    
    public string? Description { get; set; }
}