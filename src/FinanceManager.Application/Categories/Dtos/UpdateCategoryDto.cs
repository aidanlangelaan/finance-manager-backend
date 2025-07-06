namespace FinanceManager.Application.Categories.Dtos;

public class UpdateCategoryDto
{
    public required string Name { get; set; }

    public int? ParentCategoryId { get; set; }
}
