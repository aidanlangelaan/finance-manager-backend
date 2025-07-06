using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Persistence.Repositories;
using FinanceManager.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterPersistenceServices(this IServiceCollection services, IConfiguration _)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            options.UseNpgsql(config.GetConnectionString("AppDbContext"));
        });
    }
}
