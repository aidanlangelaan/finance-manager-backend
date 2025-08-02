using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Infrastructure.Identity.Services;
using FinanceManager.Infrastructure.Services;
using FinanceManager.Infrastructure.Services.FileStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<FileStorageSettings>()
                   .Configure(options => configuration.GetSection("FileStorageSettings")
                       .Bind(options));

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IUserProvisioningService, UserProvisioningService>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
