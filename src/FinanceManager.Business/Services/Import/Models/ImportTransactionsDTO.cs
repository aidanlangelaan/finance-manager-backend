using FinanceManager.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace FinanceManager.Business.Services.Import;

public class ImportTransactionsDTO
{
    public IFormFile File { get; init; }
    
    public BankType Bank { get; init; }
}