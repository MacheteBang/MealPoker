namespace Mealbot.Meals.Errors;

public static partial class Errors
{
    public static Error MealNotFoundError(Guid mealId) => Error.NotFound(
        code: "Meals.MealNotFound",
        description: $"Meal with ID {mealId} was not found.");
}
