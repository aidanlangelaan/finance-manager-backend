using FinanceManager.Application.Tags.Dtos;
using FinanceManager.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Application.Tags.Mapping;

[Mapper]
public partial class TagMapper
{
    [MapperIgnoreTarget(nameof(Tag.Id))]
    [MapperIgnoreTarget(nameof(Tag.RowVersion))]
    [MapperIgnoreTarget(nameof(Tag.CreatedOnAt))]
    [MapperIgnoreTarget(nameof(Tag.UpdatedOnAt))]
    [MapperIgnoreTarget(nameof(Tag.CreatedById))]
    [MapperIgnoreTarget(nameof(Tag.UpdatedById))]
    [MapperIgnoreTarget(nameof(Tag.CreatedBy))]
    [MapperIgnoreTarget(nameof(Tag.UpdatedBy))]
    [MapperIgnoreTarget(nameof(Tag.Transactions))]
    public partial Tag ToEntity(CreateTagDto dto);

    [MapperIgnoreSource(nameof(Tag.RowVersion))]
    [MapperIgnoreSource(nameof(Tag.CreatedOnAt))]
    [MapperIgnoreSource(nameof(Tag.UpdatedOnAt))]
    [MapperIgnoreSource(nameof(Tag.CreatedById))]
    [MapperIgnoreSource(nameof(Tag.UpdatedById))]
    [MapperIgnoreSource(nameof(Tag.CreatedBy))]
    [MapperIgnoreSource(nameof(Tag.UpdatedBy))]
    [MapperIgnoreSource(nameof(Tag.Transactions))]
    public partial TagResponseDto ToDto(Tag entity);

    [MapperIgnoreTarget(nameof(Tag.Id))]
    [MapperIgnoreTarget(nameof(Tag.RowVersion))]
    [MapperIgnoreTarget(nameof(Tag.CreatedOnAt))]
    [MapperIgnoreTarget(nameof(Tag.UpdatedOnAt))]
    [MapperIgnoreTarget(nameof(Tag.CreatedById))]
    [MapperIgnoreTarget(nameof(Tag.UpdatedById))]
    [MapperIgnoreTarget(nameof(Tag.CreatedBy))]
    [MapperIgnoreTarget(nameof(Tag.UpdatedBy))]
    [MapperIgnoreTarget(nameof(Tag.Transactions))]
    public partial void UpdateEntity(UpdateTagDto dto, Tag entity);
}
