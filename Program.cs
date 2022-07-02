var builder = WebApplication.CreateBuilder(args);

var mvcBuilder = builder.Services.AddControllers();
mvcBuilder.AddControllersAsServices();

var app = builder.Build();
app.MapControllers();
app.Run();