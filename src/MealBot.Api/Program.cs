using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Serilog.Core;

Log.Logger = CreateLoggerConfiguration();

var builder = WebApplication.CreateBuilder(args);
{
    if (builder.Environment.IsDevelopment())
    {
        builder.Configuration.AddJsonFile("appsettings.Secrets.json", true);
    }

    // Add services to the container.
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddPresentation();
    builder.Services.AddDatabaseProvider(builder.Configuration);
    builder.Services.AddIdentity(builder.Configuration);
    builder.Services.AddUsers(builder.Configuration);
    builder.Services.AddMeals();
    builder.Services.AddFamilies(builder.Configuration);
    builder.Services.AddCors(options =>
    {
        string[] corsOrigins = builder.Configuration
            .GetSection("CorsOrigins")
            .GetRequired<string[]>();

        options.AddDefaultPolicy(builder => builder
            .WithOrigins(corsOrigins)
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader());
    });

    builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
    {
        options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    app.UseSerilogRequestLogging();
    app.UseCors();
    app.UsePresentation();
    app.MapMealBotEndpoints();
    app.MapHealthChecks("/_health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
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