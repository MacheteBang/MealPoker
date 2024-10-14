namespace MealBot.Web.Common;

public static class IConfigurationExtensions
{
    /// <summary>
    /// Extracts the value with the specified key and converts it to type T.
    /// </summary>
    /// <typeparam name="T">The type to convert the value to.</typeparam>
    /// <param name="configuration">The configuration.</param>
    /// <param name="key">The key of the configuration section's value to convert.</param>
    /// <returns>The converted value or a ConfigurationEntryMissingException if the value is missing or null.</returns>
    public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
    {
        return configuration.GetValue<T>(key)
            ?? throw new ConfigurationEntryMissingException(key);
    }
}