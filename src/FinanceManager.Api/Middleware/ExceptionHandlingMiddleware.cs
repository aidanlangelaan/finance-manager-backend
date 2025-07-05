using FinanceManager.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred");

            context.Response.ContentType = "application/json";

            if (ex is MissingClaimException)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(new { Message = "Access denied." });
            }
            else if (ex is FluentValidation.ValidationException validationException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new ValidationProblemDetails(validationException.Errors.GroupBy(e => e.PropertyName).ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray()))
                {
                    Type = "https://tools.ietf.org/html/rfc7807",
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest
                });
                context.Response.ContentType = "application/problem+json";
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "An unexpected error occurred."
                });
                context.Response.ContentType = "application/json";
            }
        }
    }
}