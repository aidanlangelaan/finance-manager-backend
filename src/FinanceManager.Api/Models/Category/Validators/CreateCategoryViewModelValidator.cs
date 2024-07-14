using FluentValidation;

namespace FinanceManager.Api.Models;

public class CreateCategoryViewModelValidator : AbstractValidator<CreateCategoryViewModel>
{
    public CreateCategoryViewModelValidator()
    {
        RuleFor(category => category.Name).NotEmpty();
    }
}