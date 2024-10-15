namespace MealBot.Api.Auth.Enums;

[JsonConverter(typeof(AuthProviderConverter))]
public enum AuthProvider
{
    Unknown,
    Google,
}