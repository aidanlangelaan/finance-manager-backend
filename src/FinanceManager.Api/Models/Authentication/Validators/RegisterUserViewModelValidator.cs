using FluentValidation;

namespace FinanceManager.Api.Models;

public class RegisterUserViewModelValidator : AbstractValidator<RegisterUserViewModel>
{
    public RegisterUserViewModelValidator()
    {
        RuleFor(model => model.FirstName).NotEmpty();
        RuleFor(model => model.LastName).NotEmpty();
        RuleFor(model => model.EmailAddress).NotEmpty().EmailAddress();
    }
}