using FinanceManager.Application.Common.Models.Paging;
using FluentValidation;

namespace FinanceManager.Application.Common.Validators;

public class PagedRequestValidator : AbstractValidator<PagedRequest>
{
    public PagedRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .When(x => x.PageNumber.HasValue);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .When(x => x.PageSize.HasValue);
    }
}
