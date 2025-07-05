using FinanceManager.Domain.Entities;

namespace FinanceManager.Application.Common.Interfaces.Persistence;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(int id, int? userId, CancellationToken ct);
    IQueryable<Transaction> GetAllAsync(int? userId);
    Task AddAsync(Transaction account, CancellationToken ct);
    Task UpdateAsync(Transaction account, CancellationToken ct);
    Task DeleteAsync(Transaction account, CancellationToken ct);
    Task<Transaction?> FindEntityByIdAsync(int id, int? userId, CancellationToken ct);
}
