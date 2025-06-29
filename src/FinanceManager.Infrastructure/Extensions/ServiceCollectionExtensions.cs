using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Infrastructure.Identity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Infrastructure.Identity.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IUserProvisioningService, UserProvisioningProvisioningService>();

        return services;
    }
}
