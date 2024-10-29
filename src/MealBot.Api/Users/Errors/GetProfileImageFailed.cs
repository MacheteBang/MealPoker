namespace MealBot.Api.Users;

internal static partial class Errors
{
    public static Error GetProfileImageFailed() => Error.Failure(
        code: "User.GetProfileImageFailed",
        description: $"Unable to retrieve the user's profile image due to an internal exception.");
}
