namespace MealBot.Web.Exceptions;

public class ConfigurationEntryMissingException(string key)
    : Exception($"Configuration entry with key '{key}' is missing.");