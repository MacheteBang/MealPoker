using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MealBot.Api.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddHttpClient();

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();

        services.AddJwtAuthentications(configuration);

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

        return app;
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