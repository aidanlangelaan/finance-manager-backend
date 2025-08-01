using FinanceManager.Application.ImportJobs.Dtos;
using FluentValidation;

namespace FinanceManager.Application.ImportJobs.Validators;

// ReSharper disable once ClassNeverInstantiated.Global
public class CreateImportJobValidator : AbstractValidator<CreateImportJobDto>
{
    public CreateImportJobValidator()
    {
        RuleFor(x => x.OriginalFileName)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.MappingProfile)
            .NotNull()
            .Must(mp => mp.RootElement.ValueKind == System.Text.Json.JsonValueKind.Object)
            .WithMessage("MappingProfile must be a valid JSON object.");
    }
}
