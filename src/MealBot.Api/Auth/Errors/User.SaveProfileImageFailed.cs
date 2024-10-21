namespace MealBot.Api.Auth;

internal static partial class Errors
{
    public static Error SaveProfileImageFailed() => Error.Failure(
        code: "User.SaveProfileImageFailed",
        description: $"Unable to save the user's profile image due to an internal exception.");
}
