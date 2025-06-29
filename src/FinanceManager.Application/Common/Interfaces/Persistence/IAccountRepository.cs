using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Common.Interfaces.Persistence;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(int id, int? userId, CancellationToken ct);
    IQueryable<Account> GetAllAsync(int? userId);
    Task AddAsync(Account account, CancellationToken ct);
    Task UpdateAsync(Account account, CancellationToken ct);
    Task DeleteAsync(Account account, CancellationToken ct);
    Task<Account?> FindEntityByIdAsync(int id, int? userId, CancellationToken ct);
}