using FinanceManager.Application.Interfaces;
using FinanceManager.TestUtilities.Auth;
using FinanceManager.TestUtilities.Time;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Time.Testing;

namespace FinanceManager.Api.Tests.Common;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public TestCurrentUserService TestUser { get; } = new();

    public FakeTimeProvider FakeClock { get; } = TestClockFactory.CreateFixed(DateTimeOffset.UtcNow);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<ICurrentUserService>();
            services.AddSingleton<ICurrentUserService>(TestUser);

            services.RemoveAll<TimeProvider>();
            services.AddSingleton<TimeProvider>(FakeClock);

            services.AddAuthentication("TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });
        });
    }
}
