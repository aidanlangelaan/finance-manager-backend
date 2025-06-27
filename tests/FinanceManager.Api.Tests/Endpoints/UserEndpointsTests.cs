using System.Net;
using System.Text.Json;
using FinanceManager.Api.Tests.Common;
using Shouldly;

namespace FinanceManager.Api.Tests.Endpoints;

public class UserEndpointTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task GetMe_ShouldReturnAuthenticatedUser()
    {
        // Arrange
        factory.TestUser.KeycloakId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        factory.TestUser.DisplayName = "Test User";
        factory.TestUser.Email = "test@example.com";
        factory.TestUser.IsAuthenticated = true;

        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/user/me");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonDocument.Parse(content).RootElement;

        json.GetProperty("userId").GetString().ShouldBe("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
        json.GetProperty("name").GetString().ShouldBe("Test User");
        json.GetProperty("email").GetString().ShouldBe("test@example.com");
    }

    [Fact]
    public async Task GetMe_ShouldReturnUnauthorized_WhenUserIsNotAuthenticated()
    {
        // Arrange
        factory.TestUser.IsAuthenticated = false;
        factory.TestUser.KeycloakId = null;

        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/user/me");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
