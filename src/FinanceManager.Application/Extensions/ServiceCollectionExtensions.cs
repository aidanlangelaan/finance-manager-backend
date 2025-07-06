using FinanceManager.Application.Accounts.Interfaces;
using FinanceManager.Application.Accounts.Mapping;
using FinanceManager.Application.Accounts.Services;
using FinanceManager.Application.Accounts.Validators;
using FinanceManager.Application.Categories.Interfaces;
using FinanceManager.Application.Categories.Services;
using FinanceManager.Application.Common.Interfaces.Paging;
using FinanceManager.Application.Common.Services;
using FinanceManager.Application.Transactions.Interfaces;
using FinanceManager.Application.Transactions.Mapping;
using FinanceManager.Application.Transactions.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration _)
    {
        services.AddSingleton(TimeProvider.System);
        services.AddValidatorsFromAssemblyContaining<CreateAccountValidator>();

        // services
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<IPagingService, PagingService>();

        // mapping
        services.AddSingleton<AccountMapper>();
        services.AddSingleton<TransactionMapper>();

        return services;
    }
}
