using FinanceManager.Business.Services.Import;

namespace FinanceManager.Business.Interfaces;

public interface IImportService
{
    public Task<bool> SaveTransactions(ImportTransactionsDTO import);

    public Task<CsvImportResult?> HandleImports();
}