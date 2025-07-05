using FinanceManager.Api.Common.Validators;
using FinanceManager.Api.Extensions;
using FinanceManager.Api.ViewModels.Account.Mapping;
using FinanceManager.Application.Common.Models.Paging;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;

namespace FinanceManager.Api.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void RegisterApiServices_ShouldRegisterAllServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.RegisterApiServices();

        // Assert
        services.ShouldContainService(typeof(IHttpContextAccessor), ServiceLifetime.Singleton);
        services.ShouldContainService(typeof(AccountViewModelMapper), ServiceLifetime.Singleton);
        services.ShouldContainService(typeof(IValidator<PagedRequest>), ServiceLifetime.Scoped);
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
