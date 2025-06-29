namespace FinanceManager.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork
{
    IAccountRepository Accounts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
