using FinanceManager.Api.Common.Validators;
using FinanceManager.Api.ViewModels.Account.Mapping;
using FinanceManager.Api.ViewModels.Category.Mapping;
using FinanceManager.Api.ViewModels.ImportJob.Mapping;
using FinanceManager.Api.ViewModels.Tag.Mapping;
using FinanceManager.Api.ViewModels.Transaction.Mapping;
using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Persistence;
using FinanceManager.Persistence.Repositories;
using FinanceManager.Persistence.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddHttpContextAccessor();
        services.AddValidatorsFromAssemblyContaining<PagedRequestValidator>();

        services.AddSingleton<AccountViewModelMapper>();
        services.AddSingleton<CategoryViewModelMapper>();
        services.AddSingleton<ImportJobViewModelMapper>();
        services.AddSingleton<TagViewModelMapper>();
        services.AddSingleton<TransactionViewModelMapper>();

        return services;
    }

    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IImportJobRepository, ImportJobRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            options.UseNpgsql(config.GetConnectionString("AppDbContext"));
        });

        return services;
    }
}
