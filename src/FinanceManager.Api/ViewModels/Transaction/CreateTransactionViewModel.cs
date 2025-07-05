using FinanceManager.Domain.Enums;

namespace FinanceManager.Api.ViewModels.Transaction;

public class CreateTransactionViewModel
{
    public required int SourceAccountId { get; set; }

    public required int DestinationAccountId { get; set; }

    public required decimal Amount { get; set; }

    public required DateTime Date { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    // TODO: Add Tags property in the future
}
