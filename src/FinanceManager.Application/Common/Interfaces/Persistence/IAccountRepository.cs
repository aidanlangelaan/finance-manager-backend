using FinanceManager.Application.Accounts.Dtos;
using FinanceManager.Application.Common.Models.Paging;
using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Common.Interfaces.Persistence;

public interface IAccountRepository
{
    Task<AccountResponseDto?> GetByIdAsync(int id, Guid? keycloakId, CancellationToken ct);
    Task<PagedResult<AccountResponseDto>> GetAllAsync(Guid? keycloakId, PagedRequest paging, CancellationToken ct);
    Task AddAsync(Account account, CancellationToken ct);
    Task UpdateAsync(Account account, CancellationToken ct);
    Task DeleteAsync(Account account, CancellationToken ct);
    Task<Account?> FindEntityByIdAsync(int id, Guid? keycloakId, CancellationToken ct);
}
