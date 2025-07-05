using FinanceManager.Application.Transactions.Dtos;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Api.ViewModels.Transaction.Mapping;

[Mapper]
public partial class TransactionViewModelMapper
{
    public partial TransactionViewModel ToViewModel(TransactionResponseDto dto);
    public partial CreateTransactionDto ToDto(CreateTransactionViewModel viewModel);
    public partial UpdateTransactionDto ToDto(UpdateTransactionViewModel viewModel);
}
