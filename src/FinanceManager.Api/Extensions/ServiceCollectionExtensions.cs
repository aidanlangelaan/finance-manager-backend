using FinanceManager.Application;
using FinanceManager.Application.Extensions;
using FinanceManager.Infrastructure.Identity.Services;
using FinanceManager.Persistence.Extensions;

namespace FinanceManager.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        return services.AddApplicationServices();
    }

    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration config)
    {
        return services.AddInfrastructureServices();
    }

    public static IServiceCollection RegisterPersistenceServices(this IServiceCollection services, IConfiguration config)
    {
        return services.AddPersistenceServices(config);
    }
}