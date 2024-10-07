using FluentValidation;

namespace FinanceManager.Api.Models;

public class LoginUserViewModelValidator : AbstractValidator<LoginUserViewModel>
{
    public LoginUserViewModelValidator()
    {
        RuleFor(model => model.EmailAddress).NotEmpty().EmailAddress();
        RuleFor(model => model.Password).NotEmpty();
    }
}