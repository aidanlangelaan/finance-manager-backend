using FinanceManager.Data.Enums;

namespace FinanceManager.Api.Models;

public class ImportTransactionsViewModel
{
    public IFormFile File { get; set; }
    
    public BankType Bank { get; set; }
}