namespace MealBot.Auth.DomainErrors;

internal static partial class Errors
{
    public static Error UserNotFoundError() => Error.NotFound(
        code: "User.NotFound",
        description: $"User was not found.");
}
