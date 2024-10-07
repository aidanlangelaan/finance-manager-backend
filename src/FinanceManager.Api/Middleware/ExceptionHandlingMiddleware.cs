using FinanceManager.Business.Exceptions;

namespace FinanceManager.Api;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (UserNotFoundException ex)
        {
            logger.LogWarning(ex, "User not found.");
            await WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, "Invalid request.");
        }
        catch (InvalidTokenException ex)
        {
            logger.LogWarning(ex, "Invalid token.");
            await WriteErrorResponseAsync(context, StatusCodes.Status401Unauthorized, "Invalid request.");
        }
        catch (ConfigurationException ex)
        {
            logger.LogError(ex, "Configuration error.");
            await WriteErrorResponseAsync(context, StatusCodes.Status500InternalServerError,
                "An error occurred while processing your request. Please try again later.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred.");
            await WriteErrorResponseAsync(context, StatusCodes.Status500InternalServerError,
                "An error occurred while processing your request. Please try again later.");
        }
    }

    private static Task WriteErrorResponseAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        var errorResponse = new { Message = message };
        return context.Response.WriteAsJsonAsync(errorResponse);
    }
}