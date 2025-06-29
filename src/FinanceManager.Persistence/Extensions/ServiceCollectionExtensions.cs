using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Persistence.Repositories;
using FinanceManager.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            options.UseNpgsql(config.GetConnectionString("AppDbContext"));
        });

        return services;
    }
}
