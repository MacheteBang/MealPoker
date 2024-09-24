using MealBot.Api.Common.Errors;

namespace MealBot.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddSerilog();
        services.AddSingleton<ProblemDetailsFactory, MealBotProblemDetailsFactory>();
        return services;
    }
}