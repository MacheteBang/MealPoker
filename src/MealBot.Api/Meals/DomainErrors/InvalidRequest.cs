namespace MealBot.Api.Meals.DomainErrors;

public static partial class Errors
{
    public static Error InvalidRequest(string error) => Error.Validation(
        code: "Meals.Invalid",
        description: error);
}