using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApiServices(this IServiceCollection services)
        {
            return services;
        }
    }
}
