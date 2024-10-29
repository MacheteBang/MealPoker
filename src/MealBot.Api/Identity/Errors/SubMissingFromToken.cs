namespace MealBot.Api.Identity;

internal static partial class Errors
{
    public static Error SubMissingFromToken() => Error.Unauthorized(
        code: "Auth.SubMissingFromToken",
        description: $"Claim type `sub` is required in the token and was not found.");
}
