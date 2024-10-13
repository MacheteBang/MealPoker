namespace MealBot.Web.Common;

public static class IConfigurationExtensions
{
    public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
    {
        return configuration.GetValue<T>(key)
            ?? throw new ConfigurationEntryMissingException(key);
    }
}