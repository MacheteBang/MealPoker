namespace MealBot.Api.Families;

public static partial class Errors
{
    public static Error FamilyNotFound() => Error.NotFound(
        code: "Families.NotFound",
        description: "Family was not found.");
}