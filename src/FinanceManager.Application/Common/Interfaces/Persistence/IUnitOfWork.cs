namespace FinanceManager.Application.Common.Interfaces.Persistence;

public interface IUnitOfWork
{
    IAccountRepository Accounts { get; }

    ICategoryRepository Categories { get; }

    ITagRepository Tags { get; }

    ITransactionRepository Transactions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
