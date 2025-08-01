namespace FinanceManager.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork
{
    IAccountRepository Accounts { get; }

    ICategoryRepository Categories { get; }

    IImportJobRepository ImportJobs { get; }

    ITagRepository Tags { get; }

    ITransactionRepository Transactions { get; }

    IUserRepository Users { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
