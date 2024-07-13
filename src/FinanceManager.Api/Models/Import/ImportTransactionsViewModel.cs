using FinanceManager.Data.Enums;

namespace FinanceManager.Api.Models.Import;

public class ImportTransactionsViewModel
{
    public IFormFile File { get; set; }
    
    public BankType Bank { get; set; }
}