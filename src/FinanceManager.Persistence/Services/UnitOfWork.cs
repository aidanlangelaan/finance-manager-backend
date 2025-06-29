using FinanceManager.Application.Common.Interfaces.Persistence;

namespace FinanceManager.Persistence.Services;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}
