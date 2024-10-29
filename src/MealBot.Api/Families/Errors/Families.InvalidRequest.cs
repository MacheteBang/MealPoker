namespace MealBot.Api.Families;

public static partial class Errors
{
    public static Error InvalidRequest(string validationError) => Error.Validation(
        code: "Families.Invalid",
        description: validationError);
}