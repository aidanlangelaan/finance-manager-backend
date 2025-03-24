using MediatR;

namespace FinanceManager.Application.Features.Transactions.GetAllTransactions;

public record GetAllTransactionsQuery() : IRequest<List<GetAllTransactionsResponse>>;