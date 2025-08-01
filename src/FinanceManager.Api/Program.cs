using FinanceManager.Api.Endpoints;
using FinanceManager.Api.Extensions;
using FinanceManager.Api.Middleware;
using FinanceManager.Api.OpenApi;
using FinanceManager.Application.Extensions;
using FinanceManager.Infrastructure.Extensions;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);

builder.Services
    .RegisterApiServices()
    .RegisterApplicationServices(builder.Configuration)
    .RegisterInfrastructureServices(builder.Configuration)
    .RegisterPersistenceServices(builder.Configuration);

var authSettings = builder.Configuration.GetSection("Authentication");
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = authSettings["Authority"];
        options.Audience = authSettings["Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = authSettings["Audience"]
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddOpenApi("v1", options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddProblemDetails();
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "Personal Finance Manager - API";
    config.Version = "v1";
    config.Description = "API for managing personal finances.";

    config.AddSecurity("Bearer", [], new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Input your JWT token in this format: Bearer {your token}."
    });

    config.PostProcess = doc =>
    {
        doc.Info.Contact = new OpenApiContact
        {
            Name = "Aidan Langelaan",
            Email = "aidan@langelaan.pro"
        };
    };

    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
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
app.MapScalarApiReference((options, _) =>
{
    options.Title = "Personal Finance Manager - API";
    options.Theme = ScalarTheme.Laserwave;
    options.DefaultHttpClient = new KeyValuePair<ScalarTarget, ScalarClient>(ScalarTarget.Node, ScalarClient.Axios);
    options.WithDocumentDownloadType(DocumentDownloadType.Json);
    options.WithClientButton(false);
    options.AddPreferredSecuritySchemes("BearerAuth");
    options.WithPersistentAuthentication();
});

// Group and map endpoints
app.MapAccountEndpoints();
app.MapCategoryEndpoints();
app.MapImportJobEndpoints();
app.MapTagEndpoints();
app.MapTransactionEndpoints();
app.MapUserEndpoints();

app.Run();

public partial class Program { }

