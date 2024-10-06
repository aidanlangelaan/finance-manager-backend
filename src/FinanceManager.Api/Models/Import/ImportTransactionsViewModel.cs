using FinanceManager.Data.Enums;

namespace FinanceManager.Api.Models;

public class ImportTransactionsViewModel
{
    public IFormFile? File { get; init; }
    
    public BankType Bank { get; init; }
    
    public bool AssignCategories { get; init; }
}