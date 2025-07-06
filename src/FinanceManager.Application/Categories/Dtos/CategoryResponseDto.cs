namespace FinanceManager.Application.Categories.Dtos;

public class CategoryResponseDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public int? ParentCategoryId { get; set; }
}
