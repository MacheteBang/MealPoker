namespace MealBot.Api.Families;

public static partial class Errors
{
    public static Error UserAlreadyHasFamily() => Error.Validation(
        code: "Families.UserAlreadyHasFamily",
        description: $"User is already a member of a family.");
}
