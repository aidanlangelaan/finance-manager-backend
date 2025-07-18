using FinanceManager.Application.Common.Interfaces;
using FinanceManager.TestUtilities.Auth;
using FinanceManager.TestUtilities.Time;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using FinanceManager.Application.Accounts.Interfaces;
using FinanceManager.Application.Categories.Interfaces;
using FinanceManager.Application.Tags.Interfaces;
using Microsoft.Extensions.Time.Testing;
using Moq;

namespace FinanceManager.Api.Tests.Common;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public TestCurrentUserService TestUser { get; } = new();

    public FakeTimeProvider FakeClock { get; } = TestClockFactory.CreateFixed(DateTimeOffset.UtcNow);

    public Mock<IAccountService> AccountServiceMock { get; } = new();
    public Mock<ICategoryService> CategoryServiceMock { get; } = new();
    public Mock<ITagService> TagServiceMock { get; } = new();
    public Mock<FinanceManager.Application.Transactions.Interfaces.ITransactionService> TransactionServiceMock { get; } = new();


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<ICurrentUserService>();
            services.AddSingleton<ICurrentUserService>(TestUser);

            services.RemoveAll<IUserProvisioningService>();
            var userProvisioningServiceMock = new Mock<IUserProvisioningService>();
            userProvisioningServiceMock.Setup(s => s.GetOrCreateUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new FinanceManager.Domain.Entities.User { Id = 1, KeycloakId = Guid.NewGuid(), DisplayName = "Test User", Email = "test@example.com" });
            services.AddSingleton<IUserProvisioningService>(userProvisioningServiceMock.Object);

            services.RemoveAll<TimeProvider>();
            services.AddSingleton<TimeProvider>(FakeClock);

            services.RemoveAll<IAccountService>();
            services.AddSingleton<IAccountService>(AccountServiceMock.Object);

            services.RemoveAll<ICategoryService>();
            services.AddSingleton<ICategoryService>(CategoryServiceMock.Object);

            services.RemoveAll<ITagService>();
            services.AddSingleton<ITagService>(TagServiceMock.Object);

            services.RemoveAll<FinanceManager.Application.Transactions.Interfaces.ITransactionService>();
            services.AddSingleton<FinanceManager.Application.Transactions.Interfaces.ITransactionService>(TransactionServiceMock.Object);

            services.AddAuthentication("TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
        });
    }
}
