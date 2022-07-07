using System.Reflection;
using CalculationModelCalculator;
using Serilog;
using Serilog.Events;
using TariffComparison;

try
{
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .CreateLogger();

    Log.Information("Starting web host");
    var builder = WebApplication.CreateBuilder(args);
    ConfigureBuilder(builder.Host);
    ConfigureServices(builder.Services);

    var app = builder.Build();
    ConfigureApp(app);
    app.Run();
    return 0;
}
catch(Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

static void ConfigureBuilder(ConfigureHostBuilder builder)
{
    builder.UseSerilog();
}


static void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<Calculator>();
    services.AddScoped<ITariffStorage, TariffStorage>();
    
    services.AddSwaggerGen(config =>
    {
        var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

        var xmlDocExists = File.Exists(xmlPath);
        if (xmlDocExists)
        {
            config.IncludeXmlComments(xmlPath);
        }
    });
    
    var mvcBuilder = services.AddControllers();
    mvcBuilder.AddControllersAsServices();
} 

static void ConfigureApp(WebApplication app)
{
    app.MapControllers();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            options.RoutePrefix = string.Empty;
        });
    }
    app.UseSerilogRequestLogging();
}

/// <summary>
///  Entry point for the  WebApplicationFactory
/// </summary>
public partial class Program { }