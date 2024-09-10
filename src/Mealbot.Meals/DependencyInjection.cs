using System.Reflection;
using MealBot.Meals.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MealBot.Meals;

public static class DependencyInjection
{
    public static IServiceCollection AddMeals(this IServiceCollection services)
    {
        Assembly thisAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(mediatROptions => mediatROptions.RegisterServicesFromAssembly(thisAssembly));

        services.AddDatabase();
        services.AddScoped<IMealRepository, MealRepository>();

        return services;
    }

    public static WebApplication UseMeals(this WebApplication app)
    {
        app.MapGet(Globals.HealthRoute, () => Results.Ok());
        app.MapMealsEndpoints();

        app.SeedDatabase();

        return app;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {

        services.AddDbContext<MealsDbContext>(options =>
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var dbPath = Path.Join(path, "mealbot.meals.db");

            options.UseSqlite($"Data Source={dbPath}");
        });

        return services;
    }
    private static WebApplication SeedDatabase(this WebApplication app)
    {
        // Ensure the database is created
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MealsDbContext>();
        dbContext.Database.EnsureCreated(); // This line ensures the database is created if it doesn't exist

        return app;
    }

    private static void MapMealsEndpoints(this WebApplication app)
    {
        Assembly thisAssembly = typeof(DependencyInjection).Assembly;

        var endpoints = thisAssembly.GetTypes()
            .Where(t => typeof(MealsEndpoint).IsAssignableFrom(t) && t.IsClass && !t.IsInterface && !t.IsAbstract);

        foreach (var endpoint in endpoints)
        {
            var instance = Activator.CreateInstance(endpoint) as IEndpoint;
            instance?.AddRoutes(app);
        }
    }
}