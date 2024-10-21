namespace MealBot.Api.Auth;

internal static partial class Errors
{
    public static Error GoogleAuthorizationCodeMissing() => Error.Unauthorized(
        code: "Auth.GoogleAuthorizationCodeMissing",
        description: $"Google authorization code was expected, but not provided.");
}
