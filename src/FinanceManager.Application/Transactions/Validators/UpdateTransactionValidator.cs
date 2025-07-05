using FinanceManager.Application.Transactions.Dtos;
using FluentValidation;

namespace FinanceManager.Application.Transactions.Validators;

// ReSharper disable once UnusedType.Global
public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionDto>
{
    public UpdateTransactionValidator()
    {
        RuleFor(x => x.SourceAccountId).NotEmpty();

        RuleFor(x => x.DestinationAccountId).NotEmpty();

        // TODO: validate date not in the future
        RuleFor(x => x.Date).NotEmpty();

        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
    }
}
