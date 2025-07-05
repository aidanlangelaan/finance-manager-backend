using FinanceManager.Application.Transactions.Dtos;
using FinanceManager.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Application.Transactions.Mapping;

[Mapper]
public partial class TransactionMapper
{
    [MapperIgnoreTarget(nameof(Transaction.Id))]
    [MapperIgnoreTarget(nameof(Transaction.RowVersion))]
    [MapperIgnoreTarget(nameof(Transaction.CreatedOnAt))]
    [MapperIgnoreTarget(nameof(Transaction.UpdatedOnAt))]
    [MapperIgnoreTarget(nameof(Transaction.CreatedById))]
    [MapperIgnoreTarget(nameof(Transaction.UpdatedById))]
    [MapperIgnoreTarget(nameof(Transaction.CreatedBy))]
    [MapperIgnoreTarget(nameof(Transaction.UpdatedBy))]
    [MapperIgnoreTarget(nameof(Transaction.SourceAccount))]
    [MapperIgnoreTarget(nameof(Transaction.DestinationAccount))]
    [MapperIgnoreTarget(nameof(Transaction.Category))]
    [MapperIgnoreTarget(nameof(Transaction.Tags))]
    public partial Transaction ToEntity(CreateTransactionDto dto);

    [MapperIgnoreSource(nameof(Transaction.RowVersion))]
    [MapperIgnoreSource(nameof(Transaction.CreatedOnAt))]
    [MapperIgnoreSource(nameof(Transaction.UpdatedOnAt))]
    [MapperIgnoreSource(nameof(Transaction.CreatedById))]
    [MapperIgnoreSource(nameof(Transaction.UpdatedById))]
    [MapperIgnoreSource(nameof(Transaction.CreatedBy))]
    [MapperIgnoreSource(nameof(Transaction.UpdatedBy))]
    [MapperIgnoreSource(nameof(Transaction.SourceAccount))]
    [MapperIgnoreSource(nameof(Transaction.DestinationAccount))]
    [MapperIgnoreSource(nameof(Transaction.Category))]
    [MapperIgnoreSource(nameof(Transaction.Tags))]
    public partial TransactionResponseDto ToDto(Transaction entity);

    [MapperIgnoreTarget(nameof(Transaction.Id))]
    [MapperIgnoreTarget(nameof(Transaction.RowVersion))]
    [MapperIgnoreTarget(nameof(Transaction.CreatedOnAt))]
    [MapperIgnoreTarget(nameof(Transaction.UpdatedOnAt))]
    [MapperIgnoreTarget(nameof(Transaction.CreatedById))]
    [MapperIgnoreTarget(nameof(Transaction.UpdatedById))]
    [MapperIgnoreTarget(nameof(Transaction.CreatedBy))]
    [MapperIgnoreTarget(nameof(Transaction.UpdatedBy))]
    [MapperIgnoreTarget(nameof(Transaction.SourceAccount))]
    [MapperIgnoreTarget(nameof(Transaction.DestinationAccount))]
    [MapperIgnoreTarget(nameof(Transaction.Category))]
    [MapperIgnoreTarget(nameof(Transaction.Tags))]
    public partial void UpdateEntity(UpdateTransactionDto dto, Transaction entity);
}
