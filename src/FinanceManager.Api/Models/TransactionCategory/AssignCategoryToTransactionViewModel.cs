namespace FinanceManager.Api.Models;

public class AssignCategoryToTransactionViewModel
{
    public int TransactionId { get; set; }

    public int CategoryId { get; set; }
    
    public bool ApplyToSimilarTransactions { get; set; }
}