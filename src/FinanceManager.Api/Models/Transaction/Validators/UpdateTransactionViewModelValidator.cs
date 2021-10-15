using FluentValidation;

namespace FinanceManager.Api.Models
{
    public class UpdateTransactionViewModelValidator : AbstractValidator<UpdateTransactionViewModel>
    {
        public UpdateTransactionViewModelValidator()
        {
            RuleFor(product => product.Id).NotEmpty();
            RuleFor(product => product.Amount).NotEmpty();
            RuleFor(product => product.FromAccountId).NotEmpty();
            RuleFor(product => product.ToAccountId).NotEmpty();
            RuleFor(product => product.Date).NotEmpty();
        }
    }
}
