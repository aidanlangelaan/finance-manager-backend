using FinanceManager.Api.Models;
using FinanceManager.Api.Utils.Interceptors;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection SetupFluentValidation(this IServiceCollection services)
        {
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateTransactionViewModelValidator>());
            services.AddTransient<IValidatorInterceptor, FluentValidationInterceptor>();

            return services;
        }
    }
}
