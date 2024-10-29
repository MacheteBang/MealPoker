namespace MealBot.Api.Identity.Enums;

[JsonConverter(typeof(AuthProviderConverter))]
public enum AuthProvider
{
    Unknown,
    Google,
}