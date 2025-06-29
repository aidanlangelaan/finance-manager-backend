using System.Security.Claims;
using FinanceManager.Application.Common.Interfaces;

namespace FinanceManager.Api.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? KeycloakId
    {
        get
        {
            var sub = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(sub, out var guid) ? guid : null;
        }
    }

    public string? DisplayName
        => httpContextAccessor.HttpContext?.User.FindFirst("preferred_username")?.Value;

    public string? Email
        => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

    public bool IsAuthenticated
        => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;
}
