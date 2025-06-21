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
            var sub = context.User.FindFirst("sub")?.Value;
            var preferredUsername = context.User.FindFirst("preferred_username")?.Value;
            var email = context.User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(sub))
            {
                throw new MissingClaimException("Authenticated token is missing required 'sub' claim.");
            }

            if (string.IsNullOrWhiteSpace(preferredUsername))
            {
                throw new MissingClaimException("Authenticated token is missing required 'preferred_username' claim.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new MissingClaimException("Authenticated token is missing required 'email' claim.");
            }
            
            if (!string.IsNullOrEmpty(sub))
            {
                var localUser = await userService.GetOrCreateUserAsync(
                    Guid.Parse(sub),
                    preferredUsername,
                    email
                );

                context.Items["LocalUser"] = localUser;
                context.Items["PreferredUsername"] = preferredUsername;
                context.Items["Email"] = email;
            }
        }

        await next(context);
    }
}