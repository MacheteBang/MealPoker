namespace MealBot.Api.Identity;

internal static partial class Errors
{
    public static Error CookieMissingFromRequest(string cookieName) => Error.Unauthorized(
        code: "Auth.CookieMissingFromRequest",
        description: $"The required cookie was missing from the request. Cookie name: `{cookieName}`");
}
