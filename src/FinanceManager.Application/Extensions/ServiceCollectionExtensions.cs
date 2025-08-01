using FinanceManager.Application.Accounts.Interfaces;
using FinanceManager.Application.Accounts.Mapping;
using FinanceManager.Application.Accounts.Services;
using FinanceManager.Application.Accounts.Validators;
using FinanceManager.Application.Categories.Interfaces;
using FinanceManager.Application.Categories.Mapping;
using FinanceManager.Application.Categories.Services;
using FinanceManager.Application.Common.Interfaces.Paging;
using FinanceManager.Application.Common.Services;
using FinanceManager.Application.ImportJobs.Interfaces;
using FinanceManager.Application.ImportJobs.Mapping;
using FinanceManager.Application.ImportJobs.Services;
using FinanceManager.Application.Tags.Interfaces;
using FinanceManager.Application.Tags.Mapping;
using FinanceManager.Application.Tags.Services;
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
        services.AddScoped<IImportJobService, ImportJobService>();
        services.AddScoped<IPagingService, PagingService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<ITransactionService, TransactionService>();

        // mapping
        services.AddSingleton<AccountMapper>();
        services.AddSingleton<CategoryMapper>();
        services.AddSingleton<ImportJobMapper>();
        services.AddSingleton<TagMapper>();
        services.AddSingleton<TransactionMapper>();

        return services;
    }
}
