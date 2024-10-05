using FinanceManager.Api.Configurations;
using FinanceManager.Api.Utils;
using FinanceManager.Business;
using FinanceManager.Business.configurations;
using FinanceManager.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NSwag;

namespace FinanceManager.Api;

public class Startup(IConfiguration configuration)
{
    private IConfiguration Configuration { get; } = configuration;

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Setup dependency injection
        services.ConfigureDataServices(Configuration["ConnectionStrings:FinanceManagerContext"] ??
                                       throw new InvalidOperationException("Connection string can't be empty"))
            .ConfigureApplicationServices();

        // Adding Authentication  
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = Configuration["Authentication:ValidAudience"],
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Authentication:ValidIssuer"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        Configuration["Authentication:Secret"] ??
                        throw new InvalidOperationException("Secret can't be empty"))),
                    ValidateLifetime = true,
                    LifetimeValidator = TokenLifetimeValidator.Validate
                };
            });

        services.AddControllers();

        services.AddHsts(options => options.MaxAge = TimeSpan.FromDays(365));

        services.Configure<IISServerOptions>(options => { options.AutomaticAuthentication = false; });

        services.SetupFluentValidation();

        services.AddAutoMapper(typeof(TransactionViewModelMapperProfile), typeof(TransactionMapperProfile));

        AddSwagger(services);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseCors(options => options.AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .Build());
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        // Add OpenAPI 3.0 document serving middleware
        // Available at: http://localhost:<port>/swagger/v1/swagger.json
        app.UseOpenApi();
        
        // Add web UIs to interact with the document
        // Available at: http://localhost:<port>/swagger
        app.UseSwaggerUi();
    }

    private static void AddSwagger(IServiceCollection services)
    {
        services.AddOpenApiDocument(config =>
        {
            config.PostProcess = document =>
            {
                document.Info.Version = "v1";
                document.Info.Title = "Finance Manager - API";
                document.Info.Description = "Web API for registering and displaying financial data.";
                document.Info.Contact = new OpenApiContact
                {
                    Name = "Aidan Langelaan",
                    Email = "aidan@langelaan.pro",
                    Url = "https://twitter.com/aidanlangelaan"
                };
            };

            config.AddSecurity("Bearer", Enumerable.Empty<string>(),
                new OpenApiSecurityScheme()
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Type into the textbox: 'Bearer {your JWT token}'.",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                }
            );

            config.OperationProcessors.Add(
                new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("Bearer"));
        });
    }
}