namespace FinanceManager.Api.Models;

public class AssignCategoryToTransactionResultViewModel
{
    public int TransactionId { get; set; }

    public int CategoryId { get; set; }
    
    public int TransactionsUpdated { get; set; }
}