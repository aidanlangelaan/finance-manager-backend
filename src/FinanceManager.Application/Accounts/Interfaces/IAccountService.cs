using FinanceManager.Application.Accounts.Dtos;
using FinanceManager.Application.Common.Models.Paging;

namespace FinanceManager.Application.Accounts.Interfaces;

public interface IAccountService
{
    Task<AccountResponseDto?> GetByIdAsync(int id, CancellationToken ct);

    Task<PagedResult<AccountResponseDto>> GetAllAsync(PagedRequest paging, CancellationToken ct);

    Task<int> CreateAsync(CreateAccountDto dto, CancellationToken ct);

    Task<bool> UpdateAsync(UpdateAccountDto dto, CancellationToken ct);

    Task<bool> DeleteAsync(int id, CancellationToken ct);
}
