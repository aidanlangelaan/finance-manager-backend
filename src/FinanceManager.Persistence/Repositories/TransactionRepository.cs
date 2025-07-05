using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Persistence.Repositories;

public class TransactionRepository(AppDbContext context) : ITransactionRepository
{
    public async Task<Transaction?> GetByIdAsync(int id, int? userId, CancellationToken ct)
    {
        return await context.Transactions
            .Where(a => a.Id == id && a.CreatedById == userId)
            .FirstOrDefaultAsync(ct);
    }

    public IQueryable<Transaction> GetAllAsync(int? userId)
    {
        return context.Transactions
            .Where(a => a.CreatedById == userId)
            .OrderBy(a => a.Id);
    }

    public async Task AddAsync(Transaction account, CancellationToken ct)
    {
        await context.Transactions.AddAsync(account, ct);
    }

    public Task UpdateAsync(Transaction account, CancellationToken ct)
    {
        context.Transactions.Update(account);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Transaction account, CancellationToken ct)
    {
        context.Transactions.Remove(account);
        return Task.CompletedTask;
    }

    public async Task<Transaction?> FindEntityByIdAsync(int id, int? userId, CancellationToken ct) =>
        await context.Transactions
            .FirstOrDefaultAsync(a => a.Id == id && a.CreatedById == userId, ct);
}
