namespace MealBot.Auth.Enums;

[JsonConverter(typeof(ProviderConverter))]
public enum Provider
{
    Unknown,
    Google,
}