namespace MealBot.Auth.DomainErrors;

public static partial class Errors
{
    public static Error GoogleAuthorizationCodeExchangeFailed(string googleResponse) => Error.Unauthorized(
        code: "Auth.Google.AuthorizationCodeExchangeFailed",
        description: $"Authorization code exchange with Google failed: {googleResponse}");
}
