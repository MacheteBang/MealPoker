namespace MealBot.Api.Meals;

public static partial class Errors
{
    public static Error InvalidRating(string invalidRating) => Error.Validation(
        code: "Meals.InvalidRating",
        description: $"'{invalidRating}' is not a valid rating.");
}