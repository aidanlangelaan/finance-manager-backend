using FinanceManager.Application.Accounts.Dtos;
using FluentValidation;

namespace FinanceManager.Application.Accounts.Validators;

public class UpdateAccountValidator : AbstractValidator<UpdateAccountDto>
{
    public UpdateAccountValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
