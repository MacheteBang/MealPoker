using MealBot.Auth.Options;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace MealBot.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfigurationManager configuration)
    {
        Assembly thisAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(mediatROptions => mediatROptions.RegisterServicesFromAssembly(thisAssembly));

        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
        return services;
    }

    public static WebApplication UseAuth(this WebApplication app)
    {
        app.MapGet(Globals.HealthRoute, () => Results.Ok());
        app.MapAuthEndpoints();

        return app;
    }

    private static void MapAuthEndpoints(this WebApplication app)
    {
        Assembly thisAssembly = typeof(DependencyInjection).Assembly;

        var endpoints = thisAssembly.GetTypes()
            .Where(t => typeof(AuthEndpoint).IsAssignableFrom(t) && t.IsClass && !t.IsInterface && !t.IsAbstract);

        foreach (var endpoint in endpoints)
        {
            var instance = Activator.CreateInstance(endpoint) as IEndpoint;
            instance?.AddRoutes(app);
        }
    }
}