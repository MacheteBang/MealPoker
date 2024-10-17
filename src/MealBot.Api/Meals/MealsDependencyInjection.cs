using System.Reflection;

namespace MealBot.Api.Meals;

public static class MealsDependencyInjection
{
    public static IServiceCollection AddMeals(this IServiceCollection services)
    {
        Assembly thisAssembly = typeof(MealsDependencyInjection).Assembly;

        services.AddMediatR(mediatROptions => mediatROptions.RegisterServicesFromAssembly(thisAssembly));

        services.AddValidatorsFromAssembly(thisAssembly);

        services.AddScoped<IMealRepository, MealRepository>();

        return services;
    }
}