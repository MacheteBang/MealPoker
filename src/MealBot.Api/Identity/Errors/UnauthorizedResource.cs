namespace MealBot.Api.Identity;

internal static partial class Errors
{
    public static Error UnauthorizedResource(string resourceType, string resourceId) => Error.Unauthorized(
        code: "Auth.UnauthorizedResource",
        description: $"Access attempted to a resource the user does not have permission to access. Resource type: `{resourceType}`, Resource Id: `{resourceId}`");
}
