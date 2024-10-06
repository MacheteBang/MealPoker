namespace MealBot.Auth.DomainErrors;

public static partial class Errors
{
    public static Error GoogleIdTokenValidationFailed(string validationError) => Error.Unauthorized(
        code: "Auth.GoogleIdTokenValidationFailed",
        description: $"Google id_token validation failed: {validationError}");
}
