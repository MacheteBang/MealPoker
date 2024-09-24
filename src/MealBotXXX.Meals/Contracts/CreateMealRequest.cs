using MealBot.Meals.Features.CreateMeal;

namespace MealBot.Meals.Contracts;

public record CreateMealRequest(
    string Name,
    string? Description,
    List<MealPart> MealParts)
{
    internal CreateMealCommand ToCommand() => new(Name, Description, MealParts);
};