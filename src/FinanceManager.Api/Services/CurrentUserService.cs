using System.Security.Claims;
using FinanceManager.Application.Common.Interfaces;

namespace FinanceManager.Api.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid? KeycloakId
        => Guid.TryParse(httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id)
            ? id : null;

    public string? DisplayName
        => httpContextAccessor.HttpContext?.User.FindFirst("preferred_username")?.Value;

    public string? Email
        => httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

    public bool IsAuthenticated
        => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public int? UserId
        => httpContextAccessor.HttpContext?.Items.TryGetValue("UserId", out var value) == true && value is int id
            ? id
            : null;
}
