using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);
        
        return services;
    }
}