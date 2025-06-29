using FinanceManager.Application.Accounts.Dtos;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Api.ViewModels.Account.Mapping;

[Mapper]
public partial class AccountMapper
{
    public partial CreateAccountDto ToDto(CreateAccountViewModel vm);
    public partial UpdateAccountDto ToDto(UpdateAccountViewModel vm);
    public partial AccountViewModel ToViewModel(AccountResponseDto dto);
}
