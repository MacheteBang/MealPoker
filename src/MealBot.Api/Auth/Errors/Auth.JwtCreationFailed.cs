namespace MealBot.Api.Auth;

internal static partial class Errors
{
    public static Error JwtCreationFailed(string exceptionMessage) => Error.Failure(
        code: "Auth.JwtCreationFailed",
        description: $"JWT creation failed: {exceptionMessage}");
}