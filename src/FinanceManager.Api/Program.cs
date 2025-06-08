using FinanceManager.Api.Endpoints;
using FinanceManager.Api.Extensions;
using FinanceManager.Api.Middleware;
using NSwag;
using NSwag.Generation.Processors.Security;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationServices(builder.Configuration);
builder.Services.RegisterInfrastructureServices(builder.Configuration);
builder.Services.RegisterPersistenceServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication();
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
app.UseAuthentication();
app.UseAuthorization();

// Enable OpenAPI and Scalar API reference
app.UseOpenApi();
app.MapOpenApi();
app.MapScalarApiReference();

// Group and map endpoints
app.MapTransactionEndpointspoints();
app.MapAccountEndpoints();

app.Run();