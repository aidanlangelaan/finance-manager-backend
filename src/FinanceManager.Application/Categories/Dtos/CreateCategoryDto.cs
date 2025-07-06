namespace FinanceManager.Application.Categories.Dtos;

public class CreateCategoryDto
{
    public required string Name { get; set; }

    public int? ParentCategoryId { get; set; }
}
