using CrypticWizard.RandomWordGenerator;
using MealBot.Api.Families.Services;
using System.Reflection;

namespace MealBot.Api.Families;

public static class DependencyInjection
{
    public static IServiceCollection AddFamilies(this IServiceCollection services, IConfigurationManager configuration)
    {
        services.AddSingleton(new WordGenerator(seed: 123456));
        services.AddScoped<IFamilyService, FamilyService>();

        Assembly thisAssembly = typeof(DependencyInjection).Assembly;

        services.AddMediatR(mediatROptions => mediatROptions.RegisterServicesFromAssembly(thisAssembly));


        return services;
    }

    public static WebApplication UseFamilies(this WebApplication app) => app;
}