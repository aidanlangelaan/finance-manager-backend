using FinanceManager.Application.Transactions.Dtos;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Api.ViewModels.Transaction.Mapping;

[Mapper]
public partial class TransactionMapper
{
    public partial CreateTransactionDto ToDto(CreateTransactionViewModel vm);
    public partial UpdateTransactionDto ToDto(UpdateTransactionViewModel vm);
    public partial TransactionViewModel ToViewModel(TransactionResponseDto dto);
}
