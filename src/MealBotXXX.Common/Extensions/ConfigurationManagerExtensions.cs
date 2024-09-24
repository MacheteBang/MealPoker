using Microsoft.Extensions.Configuration;

namespace Mealbot.Common.Extensions;

public static class ConfigurationManagerExtensions
{
    public static T GetRequiredValue<T>(this IConfigurationManager configuration, string configurationKey)
    {
        return configuration.GetValue<T>(configurationKey)
            ?? throw new MissingConfigurationException(configurationKey);
    }
}