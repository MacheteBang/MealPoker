namespace MealBot.Api.Extensions;

public static class ConfigurationManagerExtensions
{
    public static T GetRequiredValue<T>(this IConfigurationManager configuration, string configurationKey)
    {
        return configuration.GetValue<T>(configurationKey)
            ?? throw new MissingConfigurationException(configurationKey);
    }
}