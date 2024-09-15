using System.Reflection;

namespace MealBot.Meals;

public static class DependencyInjection
{
    public static IServiceCollection AddMeals(this IServiceCollection services)
    {
        Assembly thisAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(mediatROptions => mediatROptions.RegisterServicesFromAssembly(thisAssembly));

        services.AddSingleton<IMealRepository, MealRepository>();

        return services;
    }

    public static WebApplication UseMeals(this WebApplication app)
    {
        app.MapGet(Globals.HealthRoute, () => Results.Ok());
        app.MapMealsEndpoints();

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