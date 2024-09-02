using Mealbot.Meals;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
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
    app.UseMeals();
}

app.Run();