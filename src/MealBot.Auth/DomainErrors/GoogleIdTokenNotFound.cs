namespace MealBot.Auth.DomainErrors;

public static partial class Errors
{
    public static Error GoogleIdTokenNotFound() => Error.Unauthorized(
        code: "Auth.Google.IdTokenNotFound",
        description: $"`id_token` not found in Google response.");
}
