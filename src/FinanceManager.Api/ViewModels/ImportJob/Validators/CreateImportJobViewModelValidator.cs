using FluentValidation;

namespace FinanceManager.Api.ViewModels.ImportJob.Validators;

public class CreateImportJobViewModelValidator : AbstractValidator<CreateImportJobViewModel>
{
    public CreateImportJobViewModelValidator()
    {
        // RuleFor(x => x.Name)
        //     .NotEmpty()
        //     .WithMessage("ImportJob name is required.")
        //     .MaximumLength(255);
        //
        // RuleFor(x => x.Type)
        //     .IsInEnum()
        //     .WithMessage("ImportJob type is required.");
        //
        // RuleFor(x => x.Description)
        //     .MaximumLength(255);
    }
}
