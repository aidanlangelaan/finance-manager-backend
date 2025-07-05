using FinanceManager.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using Moq;
using Testcontainers.PostgreSql;
using Xunit;

namespace FinanceManager.Persistence.Tests.Common;

public abstract class PersistenceTestBase : IAsyncLifetime
{
    protected AppDbContext DbContext { get; private set; } = null!;
    private PostgreSqlContainer _postgreSqlContainer = null!;
    protected Mock<ICurrentUserService> CurrentUserServiceMock { get; } = new();
    protected TimeProvider TimeProvider { get; } = new FakeTimeProvider();

    public virtual async Task InitializeAsync()
    {
        CurrentUserServiceMock.Setup(x => x.UserId).Returns(1);

        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("FinanceManagerTestDb")
            .WithUsername("admin")
            .WithPassword("admin")
            .WithCleanUp(true)
            .Build();

        await _postgreSqlContainer.StartAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_postgreSqlContainer.GetConnectionString())
            .Options;

        DbContext = new AppDbContext(options, CurrentUserServiceMock.Object, TimeProvider);
        await DbContext.Database.MigrateAsync();

        // Seed users for testing
        var testUser1 = new FinanceManager.Domain.Entities.User
        {
            Id = 1,
            KeycloakId = Guid.NewGuid(),
            DisplayName = "Test User 1",
            Email = "test1@example.com"
        };
        var testUser2 = new FinanceManager.Domain.Entities.User
        {
            Id = 2,
            KeycloakId = Guid.NewGuid(),
            DisplayName = "Test User 2",
            Email = "test2@example.com"
        };
        await DbContext.Users.AddRangeAsync(testUser1, testUser2);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task DisposeAsync()
    {
        await DbContext.DisposeAsync();
        await _postgreSqlContainer.DisposeAsync();
    }
}
