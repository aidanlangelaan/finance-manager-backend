using FluentValidation;

namespace FinanceManager.Api.Models;

public class UpdateCategoryViewModelValidator : AbstractValidator<UpdateCategoryViewModel>
{
    public UpdateCategoryViewModelValidator()
    {
        RuleFor(category => category.Id).NotEmpty();
        RuleFor(category => category.Name).NotEmpty();
    }
}