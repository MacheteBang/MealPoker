namespace MealBot.Meals.Features.CreateMeal;

public record CreateMealRequest(
    string Name,
    string? Description,
    List<MealPart> MealParts)
{
    public CreateMealCommand ToCommand() => new(Name, Description, MealParts);
};