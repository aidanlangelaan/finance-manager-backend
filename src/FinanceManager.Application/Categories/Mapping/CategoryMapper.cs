using FinanceManager.Application.Categories.Dtos;
using FinanceManager.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Application.Categories.Mapping;

[Mapper]
public partial class CategoryMapper
{
    [MapperIgnoreTarget(nameof(Category.Id))]
    [MapperIgnoreTarget(nameof(Category.RowVersion))]
    [MapperIgnoreTarget(nameof(Category.CreatedOnAt))]
    [MapperIgnoreTarget(nameof(Category.UpdatedOnAt))]
    [MapperIgnoreTarget(nameof(Category.CreatedById))]
    [MapperIgnoreTarget(nameof(Category.UpdatedById))]
    [MapperIgnoreTarget(nameof(Category.CreatedBy))]
    [MapperIgnoreTarget(nameof(Category.UpdatedBy))]
    [MapperIgnoreTarget(nameof(Category.ParentCategory))]
    [MapperIgnoreTarget(nameof(Category.Transactions))]
    [MapperIgnoreTarget(nameof(Category.Subcategories))]
    public partial Category ToEntity(CreateCategoryDto dto);

    [MapperIgnoreSource(nameof(Category.RowVersion))]
    [MapperIgnoreSource(nameof(Category.CreatedOnAt))]
    [MapperIgnoreSource(nameof(Category.UpdatedOnAt))]
    [MapperIgnoreSource(nameof(Category.CreatedById))]
    [MapperIgnoreSource(nameof(Category.UpdatedById))]
    [MapperIgnoreSource(nameof(Category.CreatedBy))]
    [MapperIgnoreSource(nameof(Category.UpdatedBy))]
    [MapperIgnoreSource(nameof(Category.ParentCategory))]
    [MapperIgnoreSource(nameof(Category.Transactions))]
    [MapperIgnoreSource(nameof(Category.Subcategories))]
    public partial CategoryResponseDto ToDto(Category entity);

    [MapperIgnoreTarget(nameof(Category.Id))]
    [MapperIgnoreTarget(nameof(Category.RowVersion))]
    [MapperIgnoreTarget(nameof(Category.CreatedOnAt))]
    [MapperIgnoreTarget(nameof(Category.UpdatedOnAt))]
    [MapperIgnoreTarget(nameof(Category.CreatedById))]
    [MapperIgnoreTarget(nameof(Category.UpdatedById))]
    [MapperIgnoreTarget(nameof(Category.CreatedBy))]
    [MapperIgnoreTarget(nameof(Category.UpdatedBy))]
    [MapperIgnoreTarget(nameof(Category.ParentCategory))]
    [MapperIgnoreTarget(nameof(Category.Transactions))]
    [MapperIgnoreTarget(nameof(Category.Subcategories))]
    public partial void UpdateEntity(UpdateCategoryDto dto, Category entity);
}
