using FinanceManager.Api.Models;
using FinanceManager.Api.Utils.Interceptors;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Api;

public static class ServiceCollectionExtensions
{
    public static void SetupFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<CreateTransactionViewModelValidator>();
            
        services.AddTransient<IValidatorInterceptor, FluentValidationInterceptor>();
    }
}