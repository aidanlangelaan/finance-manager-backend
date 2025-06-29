using FinanceManager.Application.Accounts.Dtos;
using FluentValidation;

namespace FinanceManager.Application.Accounts.Validators;

public class CreateAccountValidator : AbstractValidator<CreateAccountDto>
{
    public CreateAccountValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Type).IsInEnum();
        RuleFor(x => x.CurrentBalance).GreaterThanOrEqualTo(0);
    }
}
