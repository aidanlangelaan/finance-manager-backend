using System.Net;
using System.Text.Json;
using FinanceManager.Api.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace FinanceManager.Api.Tests.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task Invoke_ShouldHandleExceptionAndReturnInternalServerError()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        RequestDelegate next = (innerContext) => throw new Exception("Test exception");
        var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();

        var middleware = new ExceptionHandlingMiddleware(next, loggerMock.Object);

        // Act
        await middleware.Invoke(context);

        // Assert
        context.Response.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
        context.Response.ContentType.ShouldStartWith("application/json");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        var jsonResponse = JsonDocument.Parse(responseBody).RootElement;

        jsonResponse.GetProperty("type").GetString().ShouldBe("https://tools.ietf.org/html/rfc7231#section-6.6.1");
        jsonResponse.GetProperty("title").GetString().ShouldBe("Internal Server Error");
        jsonResponse.GetProperty("status").GetInt32().ShouldBe((int)HttpStatusCode.InternalServerError);
        jsonResponse.GetProperty("detail").GetString().ShouldBe("An unexpected error occurred.");
    }

    [Fact]
    public async Task Invoke_ShouldHandleValidationExceptionAndReturnBadRequest()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        var errors = new Dictionary<string, string[]>
        {
            { "Property1", new[] { "Error1", "Error2" } }
        };
        RequestDelegate next = (innerContext) => throw new ValidationException("Validation failed", errors.SelectMany(x => x.Value.Select(y => new FluentValidation.Results.ValidationFailure(x.Key, y))));
        var loggerMock = new Mock<ILogger<ExceptionHandlingMiddleware>>();

        var middleware = new ExceptionHandlingMiddleware(next, loggerMock.Object);

        // Act
        await middleware.Invoke(context);

        // Assert
        context.Response.StatusCode.ShouldBe((int)HttpStatusCode.BadRequest);
        context.Response.ContentType.ShouldStartWith("application/problem+json");

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();
        var jsonResponse = JsonDocument.Parse(responseBody).RootElement;

        jsonResponse.GetProperty("type").GetString().ShouldBe("https://tools.ietf.org/html/rfc7807");
        jsonResponse.GetProperty("title").GetString().ShouldBe("One or more validation errors occurred.");
        jsonResponse.GetProperty("status").GetInt32().ShouldBe((int)HttpStatusCode.BadRequest);
        jsonResponse.GetProperty("errors").GetProperty("Property1")[0].GetString().ShouldBe("Error1");
    }
}
