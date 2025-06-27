using System.Security.Claims;
using FinanceManager.Application.Interfaces;
using FinanceManager.Domain.Exceptions;

namespace FinanceManager.Api.Middleware;

public class UserIdentificationMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, IUserService userService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
            var name = context.User.FindFirst("name")?.Value ?? email;

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new MissingClaimException("Authenticated token is missing required 'sub' claim.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new MissingClaimException("Authenticated token is missing required 'email' claim.");
            }
            
            if (!string.IsNullOrEmpty(userId))
            {
                var localUser = await userService.GetOrCreateUserAsync(
                    Guid.Parse(userId),
                    name,
                    email
                );

                context.Items["LocalUser"] = localUser;
                context.Items["Name"] = name;
                context.Items["Email"] = email;
            }
        }

        await next(context);
    }
}