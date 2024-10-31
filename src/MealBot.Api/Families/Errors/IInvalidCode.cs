namespace MealBot.Api.Families;

public static partial class Errors
{
    public static Error InvalidCode() => Error.Validation(
        code: "Families.InvalidCode",
        description: "Invalid family code / family id combination.");
}