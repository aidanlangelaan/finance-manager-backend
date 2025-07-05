using FinanceManager.Application.Transactions.Dtos;
using FinanceManager.Application.Common.Models.Paging;

namespace FinanceManager.Application.Transactions.Interfaces;

public interface ITransactionService
{
    Task<TransactionResponseDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<PagedResult<TransactionResponseDto>> GetAllAsync(PagedRequest paging, CancellationToken ct);

    Task<int> CreateAsync(CreateTransactionDto dto, CancellationToken ct);

    Task<bool> UpdateAsync(int id, UpdateTransactionDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
