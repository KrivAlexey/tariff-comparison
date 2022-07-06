using System.Reflection;
using CalculationModelCalculator;
using TariffComparison;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);

var app = builder.Build();
ConfigureApp(app);
app.Run();

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