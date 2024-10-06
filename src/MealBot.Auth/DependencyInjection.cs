using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MealBot.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddHttpClient();

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();

        services.AddDatabaseProvider(configuration);

        Assembly thisAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(mediatROptions => mediatROptions.RegisterServicesFromAssembly(thisAssembly));

        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
        services.Configure<AuthorizationOptions>(configuration.GetSection("Authorization"));
        services.Configure<RefreshTokenOptions>(configuration.GetSection("Authorization:RefreshTokenOptions"));
        return services;
    }

    public static WebApplication UseAuth(this WebApplication app)
    {
        app.MapGet(Globals.HealthRoute, () => Results.Ok());
        app.MapAuthEndpoints();

        return app;
    }

    private static IServiceCollection AddDatabaseProvider(this IServiceCollection services, IConfigurationManager configuration)
    {
        // TODO: Convert to a robust database provider.
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        string fileLocation = System.IO.Path.Join(path, "MealBot.Auth.db");

        services.AddDbContext<AuthDbContext>(options => options.UseSqlite($"Data Source={fileLocation};"));

        using var serviceScope = services.BuildServiceProvider().CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<AuthDbContext>();
        dbContext.Database.EnsureCreated();

        return services;
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