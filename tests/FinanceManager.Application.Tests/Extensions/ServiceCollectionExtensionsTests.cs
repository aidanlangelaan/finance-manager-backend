using FinanceManager.Application.Accounts.Dtos;
using FinanceManager.Application.Accounts.Interfaces;
using FinanceManager.Application.Accounts.Mapping;
using FinanceManager.Application.Accounts.Services;
using FinanceManager.Application.Accounts.Validators;
using FinanceManager.Application.Common.Interfaces.Paging;
using FinanceManager.Application.Common.Services;
using FinanceManager.Application.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace FinanceManager.Application.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void RegisterApplicationServices_ShouldRegisterAllServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        // Act
        services.RegisterApplicationServices(configuration);

        // Assert
        services.ShouldContainService(typeof(TimeProvider), ServiceLifetime.Singleton);
        services.ShouldContainService(typeof(IAccountService), ServiceLifetime.Scoped);
        services.ShouldContainService(typeof(IPagingService), ServiceLifetime.Scoped);
        services.ShouldContainService(typeof(AccountMapper), ServiceLifetime.Singleton);
        services.ShouldContainService(typeof(IValidator<CreateAccountDto>), ServiceLifetime.Scoped);
    }
}

public static class ServiceCollectionAssertions
{
    public static void ShouldContainService(this IServiceCollection services, Type serviceType, ServiceLifetime lifetime)
    {
        var serviceDescriptor = services.FirstOrDefault(s => s.ServiceType == serviceType);
        serviceDescriptor.ShouldNotBeNull($"Service {serviceType.Name} not registered.");
        serviceDescriptor.Lifetime.ShouldBe(lifetime, $"Service {serviceType.Name} has incorrect lifetime.");
    }
}
