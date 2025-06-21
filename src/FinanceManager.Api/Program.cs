using FinanceManager.Api.Endpoints;
using FinanceManager.Api.Extensions;
using FinanceManager.Api.Middleware;
using FinanceManager.Api.Services;
using FinanceManager.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.RegisterApplicationServices(builder.Configuration);
builder.Services.RegisterInfrastructureServices(builder.Configuration);
builder.Services.RegisterPersistenceServices(builder.Configuration);

var authSettings = builder.Configuration.GetSection("Authentication");
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = authSettings["Authority"];
        options.Audience = authSettings["Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = !string.IsNullOrEmpty(authSettings["Audience"])
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Personal Finance Manager - API";
    config.Version = "v1";
    config.Description = "API for managing personal finances, including transactions and accounts.";
    
    config.PostProcess = doc =>
    {
        doc.Info.Contact = new OpenApiContact
        {
            Name = "Aidan Langelaan",
            Email = "aidan@langelaan.pro"
        };
    };
    
    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("bearer"));
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UserIdentificationMiddleware>();

// Enable OpenAPI and Scalar API reference
app.UseOpenApi();
app.MapOpenApi();
app.MapScalarApiReference();

// Group and map endpoints
app.MapTransactionEndpoints();
app.MapAccountEndpoints();
app.MapUserEndpoints();

app.Run();