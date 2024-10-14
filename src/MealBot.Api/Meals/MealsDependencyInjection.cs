using System.Reflection;

namespace MealBot.Api.Meals;

public static class MealsDependencyInjection
{
    public static IServiceCollection AddMeals(this IServiceCollection services)
    {
        Assembly thisAssembly = typeof(MealsDependencyInjection).Assembly;

        services.AddMediatR(mediatROptions => mediatROptions.RegisterServicesFromAssembly(thisAssembly));

        services.AddValidatorsFromAssembly(thisAssembly);

        services.AddSingleton<IMealRepository, MealRepository>();

        return services;
    }

    public static WebApplication UseMeals(this WebApplication app)
    {
        app.MapMealsEndpoints();

        return app;
    }

    private static void MapMealsEndpoints(this WebApplication app)
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
}