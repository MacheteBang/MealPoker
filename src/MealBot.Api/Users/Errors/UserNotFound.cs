namespace MealBot.Api.Users;

internal static partial class Errors
{
    public static Error UserNotFound() => Error.NotFound(
        code: "User.NotFound",
        description: $"User was not found.");
}
