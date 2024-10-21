namespace MealBot.Api.Auth;

internal static partial class Errors
{
    public static Error ProviderNotSupported() => Error.Validation(
        code: "Auth.ProviderNotSupported",
        description: $"Given provider is not supported.");
}
