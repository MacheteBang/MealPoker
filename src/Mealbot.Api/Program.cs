using Mealbot.Meals;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Http", LogEventLevel.Warning)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddSerilog();

    builder.Services.AddMeals();
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    app.UseSerilogRequestLogging();
    app.UseMeals();
}

app.Run();