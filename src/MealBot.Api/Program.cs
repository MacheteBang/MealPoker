using MealBot.Auth;
using MealBot.Meals;
using Serilog.Core;

Log.Logger = CreateLoggerConfiguration();

var builder = WebApplication.CreateBuilder(args);
{
    builder.Configuration.AddJsonFile("appsettings.Secrets.json", true);

    // Add services to the container.
    builder.Services.AddPresentation();
    builder.Services.AddMeals();
    builder.Services.AddAuth(builder.Configuration);

    builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    {
        options.SerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    app.UseSerilogRequestLogging();
    app.UseMeals();
    app.UseAuth();
}

app.Run();


static Logger CreateLoggerConfiguration()
{
    return new LoggerConfiguration()
        .WriteTo.Console()
        .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore.Http", LogEventLevel.Warning)
        .CreateLogger();
}