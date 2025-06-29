using System.Security.Claims;
using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Domain.Exceptions;

namespace FinanceManager.Api.Middleware;

public class UserIdentificationMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context, IUserProvisioningService userProvisioningService)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var keycloakId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
            var name = context.User.FindFirst("name")?.Value ?? email;

            if (string.IsNullOrWhiteSpace(keycloakId))
            {
                throw new MissingClaimException("Authenticated token is missing required 'sub' claim.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new MissingClaimException("Authenticated token is missing required 'email' claim.");
            }

            if (!string.IsNullOrEmpty(keycloakId))
            {
                var localUser = await userProvisioningService.GetOrCreateUserAsync(
                    Guid.Parse(keycloakId),
                    name,
                    email
                );

                context.Items["UserId"] = localUser.Id;
            }
        }

        await next(context);
    }
}
