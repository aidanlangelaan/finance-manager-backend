using FinanceManager.Application.Common.Models.Paging;
using FluentValidation;

namespace FinanceManager.Api.Common.Validators;

public class PagedRequestValidator : AbstractValidator<PagedRequest>
{
    public PagedRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0).When(x => x.PageNumber.HasValue);

        RuleFor(x => x.PageSize)
            .GreaterThan(0).LessThanOrEqualTo(100).When(x => x.PageSize.HasValue);
    }
}
