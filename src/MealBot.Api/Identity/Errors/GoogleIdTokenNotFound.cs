namespace MealBot.Api.Identity;

internal static partial class Errors
{
    public static Error GoogleIdTokenNotFound() => Error.Unauthorized(
        code: "Auth.GoogleIdTokenNotFound",
        description: $"'id_token' not found in the response.");
}
