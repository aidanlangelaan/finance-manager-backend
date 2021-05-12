using Microsoft.Extensions.DependencyInjection;
using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services;

namespace FinanceManager.Business
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services) =>
            services.AddTransient<ITransactionService, TransactionService>();
    }
}
