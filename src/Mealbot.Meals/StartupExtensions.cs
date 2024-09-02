

using Microsoft.AspNetCore.Http;

namespace Mealbot.Meals;

public static class StartupExtensions
{
    public static IServiceCollection AddMeals(this IServiceCollection services)
    {
        return services;
    }

    public static WebApplication UseMeals(this WebApplication app)
    {
        app.MapGet(Globals.HealthRoute, () => Results.Ok());

        return app;
    }
}