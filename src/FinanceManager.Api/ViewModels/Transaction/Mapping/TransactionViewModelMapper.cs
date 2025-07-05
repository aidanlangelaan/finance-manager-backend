using FinanceManager.Application.Transactions.Dtos;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Api.ViewModels.Transaction.Mapping;

[Mapper]
public partial class TransactionViewModelMapper
{
    public partial TransactionViewModel ToViewModel(TransactionResponseDto dto);
}
