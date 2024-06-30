using FluentValidation;

namespace FinanceManager.Api.Models;

public class LoginUserViewModelValidator : AbstractValidator<LoginUserViewModel>
{
    public LoginUserViewModelValidator()
    {
        RuleFor(login => login.EmailAddress).NotEmpty();
        RuleFor(login => login.Password).NotEmpty();
    }
}