using FinanceManager.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Data;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureDataServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<FinanceManagerDbContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mysqlOptions =>
            {
                mysqlOptions.EnableStringComparisonTranslations();
            }),
            ServiceLifetime.Singleton);

        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<FinanceManagerDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}