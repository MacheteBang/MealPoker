using System.Reflection;

namespace MealBot.Api.Meals;

public static class MealsDependencyInjection
{
    public static IServiceCollection AddMeals(this IServiceCollection services)
    {
        services.AddScoped<IMealsService, MealsService>();

        return services;
    }

    public static WebApplication UseMeals(this WebApplication app) => app;
}