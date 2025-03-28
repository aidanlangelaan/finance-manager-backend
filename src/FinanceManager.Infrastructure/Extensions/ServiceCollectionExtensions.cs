using FinanceManager.Domain.Interfaces;
using FinanceManager.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        
        return services;
    }
}