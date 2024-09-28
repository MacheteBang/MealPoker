namespace MealBot.Auth.DomainErrors;

public static partial class Errors
{
    public static Error UserNotFoundError() => Error.NotFound(
        code: "User.NotFound",
        description: $"User was not found.");
}
