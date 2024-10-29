using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Reflection;

namespace MealBot.Api.Identity;

public static class DependencyInjection
{
    public static IServiceCollection AddAuth(this IServiceCollection services, IConfigurationManager configuration)
    {
        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddHttpClient();

        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProfileImageStorageService, LocalProfileImageStorageService>();

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
        JwtOptions jwtOptions = configuration.GetSection("Authorization:JwtOptions").GetRequired<JwtOptions>();
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