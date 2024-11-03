namespace MealBot.Api.Meals;

public static class MealsDependencyInjection
{
    public static IServiceCollection AddMeals(this IServiceCollection services) => services;

    public static WebApplication UseMeals(this WebApplication app) => app;
}