using System.Security.Claims;
using FinanceManager.Api.Middleware;
using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Domain.Entities;
using FinanceManager.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;

namespace FinanceManager.Api.Tests.Middleware;

public class UserIdentificationMiddlewareTests
{
    private readonly Mock<IUserProvisioningService> _userProvisioningServiceMock;
    private readonly UserIdentificationMiddleware _middleware;

    public UserIdentificationMiddlewareTests()
    {
        _userProvisioningServiceMock = new Mock<IUserProvisioningService>();
        _middleware = new UserIdentificationMiddleware(next: (innerContext) => Task.CompletedTask);
    }

    [Fact]
    public async Task Invoke_ShouldSetUserIdInContextItems_WhenUserIsAuthenticatedAndClaimsArePresent()
    {
        // Arrange
        var keycloakId = Guid.NewGuid();
        var email = "test@example.com";
        var name = "Test User";
        var userId = 1;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, keycloakId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim("name", name)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var context = new DefaultHttpContext { User = principal };

        _userProvisioningServiceMock.Setup(s => s.GetOrCreateUserAsync(keycloakId, name, email))
            .ReturnsAsync(new User { Id = userId, KeycloakId = keycloakId, DisplayName = name, Email = email });

        // Act
        await _middleware.Invoke(context, _userProvisioningServiceMock.Object);

        // Assert
        context.Items["UserId"].ShouldBe(userId);
    }

    [Fact]
    public async Task Invoke_ShouldNotSetUserIdInContextItems_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var context = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) };

        // Act
        await _middleware.Invoke(context, _userProvisioningServiceMock.Object);

        // Assert
        context.Items.ShouldNotContainKey("UserId");
    }

    [Fact]
    public async Task Invoke_ShouldThrowMissingClaimException_WhenNameIdentifierClaimIsMissing()
    {
        // Arrange
        var email = "test@example.com";
        var name = "Test User";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email),
            new Claim("name", name)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var context = new DefaultHttpContext { User = principal };

        // Act & Assert
        await Should.ThrowAsync<MissingClaimException>(
            async () => await _middleware.Invoke(context, _userProvisioningServiceMock.Object));
    }

    [Fact]
    public async Task Invoke_ShouldThrowMissingClaimException_WhenEmailClaimIsMissing()
    {
        // Arrange
        var keycloakId = Guid.NewGuid();
        var name = "Test User";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, keycloakId.ToString()),
            new Claim("name", name)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var principal = new ClaimsPrincipal(identity);

        var context = new DefaultHttpContext { User = principal };

        // Act & Assert
        await Should.ThrowAsync<MissingClaimException>(
            async () => await _middleware.Invoke(context, _userProvisioningServiceMock.Object));
    }
}
