using FinanceManager.Application;
using FinanceManager.Persistence.Extensions;

namespace FinanceManager.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        // Register application-level services (CQRS, Validators, etc.)
        return services.AddApplicationServices();
    }

    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        //return services.AddInfrastructure(config); // Handles logging, file, email, etc.

        return services;
    }

    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration config)
    {
        return services.AddPersistenceServices(config);
    }
}