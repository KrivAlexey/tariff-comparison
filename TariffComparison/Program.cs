var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
var mvcBuilder = builder.Services.AddControllers();
mvcBuilder.AddControllersAsServices();

var app = builder.Build();
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
app.Run();