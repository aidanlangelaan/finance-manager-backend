namespace FinanceManager.Business.Services.Models;

public class AssignCategoryToTransactionDTO
{
    public int TransactionId { get; set; }

    public int CategoryId { get; set; }
    
    public bool ApplyToSimilarTransactions { get; set; }
}