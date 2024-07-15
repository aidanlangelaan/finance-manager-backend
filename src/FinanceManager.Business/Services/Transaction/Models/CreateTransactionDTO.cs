namespace FinanceManager.Business.Services.Models;

public class CreateTransactionDTO
{
    public double Amount { get; set; }

    public int FromAccountId { get; set; }

    public int ToAccountId { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }
}