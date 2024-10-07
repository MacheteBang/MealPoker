using Microsoft.AspNetCore.Authentication.JwtBearer;
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

        services.AddJwtAuthentications(configuration);

        services.AddDatabaseProvider(configuration);

        Assembly thisAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(mediatROptions => mediatROptions.RegisterServicesFromAssembly(thisAssembly));

        services.Configure<AuthenticationOptions>(configuration.GetSection("Authentication"));
        services.Configure<AuthorizationOptions>(configuration.GetSection("Authorization"));
        services.Configure<RefreshTokenOptions>(configuration.GetSection("Authorization:RefreshTokenOptions"));
        services.Configure<JwtOptions>(configuration.GetSection("Authorization:JwtOptions"));

        return services;
    }

    public static WebApplication UseAuth(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

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

    private static IServiceCollection AddJwtAuthentications(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("Authorization:JwtOptions").Get<JwtOptions>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.ValidIssuer,
                ValidAudience = jwtOptions.ValidAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.IssuerSigningKey))
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Bearer", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
            });
        });

        return services;
    }
}