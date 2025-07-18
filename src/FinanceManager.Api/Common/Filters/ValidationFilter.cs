
using FluentValidation;

namespace FinanceManager.Api.Common.Filters;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.FirstOrDefault(x => x?.GetType() == typeof(T));

        if (argument is null)
        {
            return await next(context);
        }
        
        var validationResult = await validator.ValidateAsync((T)argument, context.HttpContext.RequestAborted);

        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }
        
        return await next(context);
    }
}
