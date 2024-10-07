namespace MealBot.Auth.Enums;

[JsonConverter(typeof(AuthProviderConverter))]
public enum AuthProvider
{
    Unknown,
    Google,
}