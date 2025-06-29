using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
#pragma warning disable CS0618 // Type or member is obsolete

namespace FinanceManager.Api.Tests.Common;

public class TestAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee"),
            new Claim("name", "Test User"),
            new Claim(ClaimTypes.Email, "test@example.com")
        };

        var identity = new ClaimsIdentity(claims, "TestScheme");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "TestScheme");

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
