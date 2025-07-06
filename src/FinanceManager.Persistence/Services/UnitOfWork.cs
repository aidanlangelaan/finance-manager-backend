using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Persistence.Repositories;

namespace FinanceManager.Persistence.Services;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public IAccountRepository Accounts { get; } = new AccountRepository(context);

    public ICategoryRepository Categories { get; } = new CategoryRepository(context);

    public ITransactionRepository Transactions { get; } = new TransactionRepository(context);

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => context.SaveChangesAsync(ct);
}
