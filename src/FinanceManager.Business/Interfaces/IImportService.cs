using FinanceManager.Business.Services.Models;

namespace FinanceManager.Business.Interfaces;

public interface IImportService
{
    public Task<List<GetImportDTO>> GetAll();

    public Task<GetImportDTO?> GetById(int id);
    
    public Task<bool> SaveTransactions(ImportTransactionsDTO import);

    public Task<CsvImportResult?> HandleImports();
}