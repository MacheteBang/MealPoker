namespace Mealbot.Common.Exceptions;

public class MissingConfigurationException(string configurationKey)
    : Exception($"Configuration key '{configurationKey}' is missing")
{
    private readonly string _configurationKey = configurationKey;

    public string ConfigurationKey => _configurationKey;

    public override string Message => $"Configuration key '{_configurationKey}' is missing.";

    public override string ToString()
    {
        return $"{base.ToString()}, ConfigurationKey: {_configurationKey}";
    }
}