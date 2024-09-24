namespace MealBot.Auth;

public static class DependecyInjection
{
    public static IServiceCollection AddAuth(this IServiceCollection services) => services;

    public static WebApplication UseAuth(this WebApplication app)
    {
        app.MapGet(Globals.HealthRoute, () => Results.Ok());

        return app;
    }
}