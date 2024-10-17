using MealBot.Api.Common.Errors;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MealBot.Api;

public static class GlobalDependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSerilog();
        services.AddSingleton<ProblemDetailsFactory, MealBotProblemDetailsFactory>();
        return services;
    }

    public static WebApplication UsePresentation(this WebApplication app)
    {
        app.UseTestEndpoints();

        return app;
    }

    public static IServiceCollection AddDatabaseProvider(this IServiceCollection services, IConfigurationManager configuration)
    {
        // TODO: Convert to a robust database provider.
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var directoryPath = Path.Join(Environment.GetFolderPath(folder), ".MealBot");
        string fileLocation = System.IO.Path.Join(directoryPath, "MealBot.Api.Auth.db");

        // Create directory if it doesn't exist
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        services.AddDbContext<AuthDbContext>(options => options.UseSqlite($"Data Source={fileLocation};"));

        using var serviceScope = services.BuildServiceProvider().CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>();
        dbContext.Database.EnsureCreated();

        return services;
    }

    public static void MapMealBotEndpoints(this WebApplication app)
    {
        Assembly thisAssembly = typeof(MealsDependencyInjection).Assembly;

        var endpoints = thisAssembly.GetTypes()
            .Where(t => typeof(MealBotEndpoint).IsAssignableFrom(t) && t.IsClass && !t.IsInterface && !t.IsAbstract);

        foreach (var endpoint in endpoints)
        {
            var instance = Activator.CreateInstance(endpoint) as IMealBotEndpoint;
            instance?.AddRoutes(app);
        }
    }

    private static WebApplication UseTestEndpoints(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapGet("/test/show-cookies", (HttpContext context) =>
            {
                return Results.Ok(context.Request.Cookies);
            });
        }

        return app;
    }
}