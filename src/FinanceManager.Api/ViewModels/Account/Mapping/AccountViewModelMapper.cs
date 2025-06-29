using FinanceManager.Application.Accounts.Dtos;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Api.ViewModels.Account.Mapping;

[Mapper]
public partial class AccountViewModelMapper
{
    public partial AccountViewModel ToViewModel(AccountResponseDto dto);
}
