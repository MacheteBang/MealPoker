namespace MealBot.Auth.DomainErrors;

internal static partial class Errors
{
    public static Error GoogleIdTokenValidationFailed(string validationError) => Error.Unauthorized(
        code: "Auth.GoogleIdTokenValidationFailed",
        description: $"Google id_token validation failed: {validationError}");
}
