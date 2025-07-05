using FinanceManager.Application.Transactions.Dtos;
using FluentValidation;

namespace FinanceManager.Application.Transactions.Validators;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateTransactionValidator : AbstractValidator<CreateTransactionDto>
{
    public CreateTransactionValidator()
    {
        RuleFor(x => x.SourceAccountId).NotEmpty();

        RuleFor(x => x.DestinationAccountId).NotEmpty();

        // TODO: validate date not in the future
        RuleFor(x => x.Date).NotEmpty();

        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
}
