using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureDataServices(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<FinanceManagerDbContext>(options => options.UseSqlServer(connectionString));
    }
}
