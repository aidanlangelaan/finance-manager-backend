using FinanceManager.Application.Accounts.Interfaces;
using FinanceManager.Application.Accounts.Mapping;
using FinanceManager.Application.Accounts.Services;
using FinanceManager.Application.Accounts.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddValidatorsFromAssemblyContaining<CreateAccountValidator>();

        // services
        services.AddScoped<IAccountService, AccountService>();

        // mapping
        services.AddSingleton<AccountMapper>();

        return services;
    }
}
