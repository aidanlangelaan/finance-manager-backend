using FinanceManager.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FinanceManager.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public bool IsAuthenticated
        => httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated
           ?? false;
    
    public string? UserId =>
        httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value;

    public string? UserName
        => httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;
    
    public string? UserEmail
        => httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
}