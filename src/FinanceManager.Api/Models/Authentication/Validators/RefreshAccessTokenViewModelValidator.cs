using FluentValidation;

namespace FinanceManager.Api.Models;

public class RefreshAccessTokenViewModelValidator : AbstractValidator<RefreshAccessTokenViewModel>
{
    public RefreshAccessTokenViewModelValidator()
    {
        RuleFor(model => model.EmailAddress).NotEmpty().EmailAddress();
    }
}