namespace MealBot.Api.Identity;

internal static partial class Errors
{
    public static Error GoogleAuthorizationCodeExchangeFailed(string responseMessage) => Error.Unauthorized(
        code: "Auth.GoogleAuthorizationCodeExchangeFailed",
        description: $"Authorization code exchange failed. {responseMessage}");
}
