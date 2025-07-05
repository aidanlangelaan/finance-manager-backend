
using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Domain.Entities;
using FinanceManager.Infrastructure.Identity.Services;
using FinanceManager.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;
using Moq;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace FinanceManager.Infrastructure.Tests.Identity.Services;

public class UserProvisioningProvisioningServiceTests
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly TimeProvider _timeProvider;

    public UserProvisioningProvisioningServiceTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _timeProvider = new FakeTimeProvider();
        _currentUserServiceMock.Setup(x => x.UserId).Returns(1);
    }

    private AppDbContext CreateContext() => new AppDbContext(_dbContextOptions, _currentUserServiceMock.Object, _timeProvider);

    [Fact]
    public async Task GetOrCreateUserAsync_ShouldCreateUser_WhenUserDoesNotExist()
    {
        // Arrange
        var keycloakId = Guid.NewGuid();
        var name = "Test User";
        var email = "test@example.com";

        await using var context = CreateContext();
        var sut = new UserProvisioningProvisioningService(context);

        // Act
        var result = await sut.GetOrCreateUserAsync(keycloakId, name, email);

        // Assert
        result.ShouldNotBeNull();
        result.KeycloakId.ShouldBe(keycloakId);
        result.DisplayName.ShouldBe(name);
        result.Email.ShouldBe(email);

        var userInDb = await context.Users.FindAsync(result.Id);
        userInDb.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetOrCreateUserAsync_ShouldReturnExistingUser_WhenUserExists()
    {
        // Arrange
        var keycloakId = Guid.NewGuid();
        var name = "Test User";
        var email = "test@example.com";
        var user = new User { KeycloakId = keycloakId, DisplayName = name, Email = email };

        await using var context = CreateContext();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var sut = new UserProvisioningProvisioningService(context);

        // Act
        var result = await sut.GetOrCreateUserAsync(keycloakId, "New Name", "new@example.com");

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(user.Id);
    }

    [Fact]
    public async Task GetOrCreateUserAsync_ShouldUpdateUser_WhenUserDataChanges()
    {
        // Arrange
        var keycloakId = Guid.NewGuid();
        var name = "Test User";
        var email = "test@example.com";
        var user = new User { KeycloakId = keycloakId, DisplayName = name, Email = email };

        await using var context = CreateContext();
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var sut = new UserProvisioningProvisioningService(context);

        // Act
        var result = await sut.GetOrCreateUserAsync(keycloakId, "New Name", "new@example.com");

        // Assert
        result.DisplayName.ShouldBe("New Name");
        result.Email.ShouldBe("new@example.com");
    }

    [Fact]
    public async Task GetOrCreateUserAsync_ShouldThrowException_WhenNameIsMissingForNewUser()
    {
        // Arrange
        var keycloakId = Guid.NewGuid();
        var email = "test@example.com";

        await using var context = CreateContext();
        var sut = new UserProvisioningProvisioningService(context);

        // Act & Assert
        await Should.ThrowAsync<InvalidOperationException>(() => sut.GetOrCreateUserAsync(keycloakId, null, email));
    }

    [Fact]
    public async Task GetOrCreateUserAsync_ShouldThrowException_WhenEmailIsMissingForNewUser()
    {
        // Arrange
        var keycloakId = Guid.NewGuid();
        var name = "Test User";

        await using var context = CreateContext();
        var sut = new UserProvisioningProvisioningService(context);

        // Act & Assert
        await Should.ThrowAsync<InvalidOperationException>(() => sut.GetOrCreateUserAsync(keycloakId, name, null));
    }
}
