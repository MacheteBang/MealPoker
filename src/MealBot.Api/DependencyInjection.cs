using MealBot.Api.Common.Errors;
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
        string connectionString = configuration.GetRequiredValue<string>("ConnectionStrings:Database");
        services.AddDbContext<MealBotDbContext>(options => options.UseSqlServer(connectionString));

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