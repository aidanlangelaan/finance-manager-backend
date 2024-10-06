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

        services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<FinanceManagerDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<User>>(TokenOptions.DefaultProvider)
            .AddTokenProvider<EmailTokenProvider<User>>("Email");

        return services;
    }
}