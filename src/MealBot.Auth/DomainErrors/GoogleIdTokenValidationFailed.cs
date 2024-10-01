namespace MealBot.Auth.DomainErrors;

public static partial class Errors
{
    public static Error GoogleIdTokenValidationFailed(string message) => Error.Unauthorized(
        code: "Auth.Google.IdTokenValidationFailed",
        description: $"`id_token` found in Google response did not pass validation: {message}");
}
