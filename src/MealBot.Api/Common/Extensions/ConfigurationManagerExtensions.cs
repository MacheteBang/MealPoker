namespace MealBot.Api.Common.Extensions;

public static class ConfigurationManagerExtensions
{
    public static T GetRequired<T>(this IConfiguration configuration)
    {
        return configuration.Get<T>()
            ?? throw new MissingConfigurationException(typeof(T).Name);
    }
    public static T GetRequiredValue<T>(this IConfigurationManager configuration, string configurationKey)
    {
        return configuration.GetValue<T>(configurationKey)
            ?? throw new MissingConfigurationException(configurationKey);
    }

    public static IConfigurationSection GetRequiredSection(this IConfigurationManager configuration, string sectionKey)
    {
        var section = configuration.GetSection(sectionKey);
        if (!section.Exists())
        {
            throw new MissingConfigurationException(sectionKey);
        }

        return section;
    }
}