namespace FinanceManager.Application.Features.Transactions.GetAllTransactions;

public record GetAllTransactionsResponse(Guid Id, decimal Amount, string Category);