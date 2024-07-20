using FinanceManager.Data.Enums;
using Microsoft.AspNetCore.Http;

namespace FinanceManager.Business.Services.Models;

public class ImportTransactionsDTO
{
    public IFormFile File { get; init; }
    
    public BankType Bank { get; init; }
    
    public bool AssignCategories { get; init; }
}