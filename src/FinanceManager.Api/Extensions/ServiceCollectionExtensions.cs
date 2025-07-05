using FinanceManager.Api.Common.Validators;
using FinanceManager.Api.ViewModels.Account.Mapping;
using FinanceManager.Api.ViewModels.Transaction.Mapping;
using FluentValidation;

namespace FinanceManager.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddValidatorsFromAssemblyContaining<PagedRequestValidator>();

        services.AddSingleton<AccountViewModelMapper>();
        services.AddSingleton<TransactionViewModelMapper>();

        return services;
    }
}
