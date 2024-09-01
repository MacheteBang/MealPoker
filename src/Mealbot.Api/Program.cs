Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddSerilog();
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
}

app.Run();