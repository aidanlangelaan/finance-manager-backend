using System.Security.Claims;
using FinanceManager.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;

namespace FinanceManager.Infrastructure.Tests.Services;

public class CurrentUserServiceTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly CurrentUserService _sut;

    public CurrentUserServiceTests()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _sut = new CurrentUserService(_httpContextAccessorMock.Object);
    }

    [Fact]
    public void KeycloakId_ShouldReturnNull_WhenClaimIsMissing()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _sut.KeycloakId;

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void KeycloakId_ShouldReturnGuid_WhenClaimIsValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var claims = new[] { new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = user };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _sut.KeycloakId;

        // Assert
        result.ShouldBe(userId);
    }

    [Fact]
    public void DisplayName_ShouldReturnNull_WhenClaimIsMissing()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _sut.DisplayName;

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void DisplayName_ShouldReturnName_WhenClaimExists()
    {
        // Arrange
        var name = "testuser";
        var claims = new[] { new Claim("preferred_username", name) };
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = user };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _sut.DisplayName;

        // Assert
        result.ShouldBe(name);
    }

    [Fact]
    public void Email_ShouldReturnNull_WhenClaimIsMissing()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _sut.Email;

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void Email_ShouldReturnEmail_WhenClaimExists()
    {
        // Arrange
        var email = "test@example.com";
        var claims = new[] { new Claim(ClaimTypes.Email, email) };
        var identity = new ClaimsIdentity(claims);
        var user = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = user };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _sut.Email;

        // Assert
        result.ShouldBe(email);
    }

    [Fact]
    public void IsAuthenticated_ShouldReturnFalse_WhenUserIsNotAuthenticated()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _sut.IsAuthenticated;

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsAuthenticated_ShouldReturnTrue_WhenUserIsAuthenticated()
    {
        // Arrange
        var identity = new ClaimsIdentity(new Claim[] { }, "test");
        var user = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = user };
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _sut.IsAuthenticated;

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void UserId_ShouldReturnNull_WhenItemIsMissing()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _sut.UserId;

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void UserId_ShouldReturnId_WhenItemExists()
    {
        // Arrange
        var userId = 123;
        var httpContext = new DefaultHttpContext();
        httpContext.Items["UserId"] = userId;
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _sut.UserId;

        // Assert
        result.ShouldBe(userId);
    }
}