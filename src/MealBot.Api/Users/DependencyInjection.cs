namespace MealBot.Api.Users;

public static class DependencyInjection
{
    public static IServiceCollection AddUsers(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProfileImageStorageService, LocalProfileImageStorageService>();

        return services;
    }

    public static WebApplication UseUsers(this WebApplication app) => app;
}