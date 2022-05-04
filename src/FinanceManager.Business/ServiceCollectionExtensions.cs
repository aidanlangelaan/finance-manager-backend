using FinanceManager.Business.Interfaces;
using FinanceManager.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Business
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IImportService, ImportService>();
            services.AddTransient<ITransactionService, TransactionService>();

            return services;
        }
    }
}
