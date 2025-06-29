using FinanceManager.Application.Accounts.Dtos;
using FinanceManager.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Application.Accounts.Mapping;

[Mapper]
public partial class AccountMapper
{
    [MapperIgnoreTarget(nameof(Account.Id))]
    [MapperIgnoreTarget(nameof(Account.RowVersion))]
    [MapperIgnoreTarget(nameof(Account.CreatedOnAt))]
    [MapperIgnoreTarget(nameof(Account.UpdatedOnAt))]
    [MapperIgnoreTarget(nameof(Account.CreatedById))]
    [MapperIgnoreTarget(nameof(Account.UpdatedById))]
    [MapperIgnoreTarget(nameof(Account.CreatedBy))]
    [MapperIgnoreTarget(nameof(Account.UpdatedBy))]
    [MapperIgnoreTarget(nameof(Account.SourceTransactions))]
    [MapperIgnoreTarget(nameof(Account.DestinationTransactions))]
    public partial Account ToEntity(CreateAccountDto dto);

    [MapperIgnoreSource(nameof(Account.RowVersion))]
    [MapperIgnoreSource(nameof(Account.CreatedOnAt))]
    [MapperIgnoreSource(nameof(Account.UpdatedOnAt))]
    [MapperIgnoreSource(nameof(Account.CreatedById))]
    [MapperIgnoreSource(nameof(Account.UpdatedById))]
    [MapperIgnoreSource(nameof(Account.CreatedBy))]
    [MapperIgnoreSource(nameof(Account.UpdatedBy))]
    [MapperIgnoreSource(nameof(Account.SourceTransactions))]
    [MapperIgnoreSource(nameof(Account.DestinationTransactions))]
    public partial AccountResponseDto ToDto(Account entity);

    [MapperIgnoreTarget(nameof(Account.CurrentBalance))]
    [MapperIgnoreTarget(nameof(Account.RowVersion))]
    [MapperIgnoreTarget(nameof(Account.CreatedOnAt))]
    [MapperIgnoreTarget(nameof(Account.UpdatedOnAt))]
    [MapperIgnoreTarget(nameof(Account.CreatedById))]
    [MapperIgnoreTarget(nameof(Account.UpdatedById))]
    [MapperIgnoreTarget(nameof(Account.CreatedBy))]
    [MapperIgnoreTarget(nameof(Account.UpdatedBy))]
    [MapperIgnoreTarget(nameof(Account.SourceTransactions))]
    [MapperIgnoreTarget(nameof(Account.DestinationTransactions))]
    public partial void UpdateEntity(UpdateAccountDto dto, Account entity);
}
