using FluentValidation;

namespace FinanceManager.Api.Models;

public class CreateTransactionViewModelValidator : AbstractValidator<CreateTransactionViewModel>
{
    public CreateTransactionViewModelValidator()
    {
        RuleFor(transaction => transaction.Amount).NotEmpty();
        RuleFor(transaction => transaction.FromAccountId).NotEmpty();
        RuleFor(transaction => transaction.ToAccountId).NotEmpty();
        RuleFor(transaction => transaction.Date).NotEmpty();
    }
}