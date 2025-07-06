using FluentValidation;

namespace FinanceManager.Api.ViewModels.Category.Validators;


// ReSharper disable once UnusedType.Global
public class CreateCategoryViewModelValidator : AbstractValidator<CreateCategoryViewModel>
{
    public CreateCategoryViewModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Category name is required.")
            .MaximumLength(255);
    }
}
