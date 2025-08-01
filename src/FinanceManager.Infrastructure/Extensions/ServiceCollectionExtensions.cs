using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Infrastructure.Identity.Services;
using FinanceManager.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IUserProvisioningService, UserProvisioningService>();
        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
