namespace MealBot.Api.Meals;

public static partial class Errors
{
    public static Error InvalidRequest(string validationError) => Error.Validation(
        code: "Meals.Invalid",
        description: validationError);
}