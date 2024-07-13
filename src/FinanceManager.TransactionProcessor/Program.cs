using FinanceManager.Api.Configurations;
using FinanceManager.Business;
using FinanceManager.Business.configurations;
using FinanceManager.Data;
using FinanceManager.TransactionProcessor;
using Serilog;
using Serilog.Templates;
using Serilog.Templates.Themes;

try
{
    var configuration = new ConfigurationBuilder()
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddJsonFile($"appsettings.local.json", optional: true)
        .Build();

    var builder = Host
        .CreateDefaultBuilder(args)
        .UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(new ExpressionTemplate(
                    // Include trace and span ids when present.
                    "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}",
                    theme: TemplateTheme.Code));
        });

    builder.ConfigureAppConfiguration((_, config) =>
        {
            config.Sources.Clear();
            config.AddConfiguration(configuration);
        })
        .ConfigureServices(services =>
        {
            services.AddHostedService<Worker>();
            services.ConfigureDataServices(configuration["ConnectionStrings:FinanceManagerContext"] ??
                                           throw new InvalidOperationException("Connection string can't be empty"));
            services.ConfigureApplicationServices();
            services.AddAutoMapper(typeof(TransactionViewModelMapperProfile), typeof(TransactionMapperProfile));
        });

    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
}
finally
{
    await Log.CloseAndFlushAsync();
}