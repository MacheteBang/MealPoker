namespace MealBot.Meals.Features.UpdateMeal;

public record UpdateMealRequest(
    string Name,
    string? Description,
    List<MealPart> MealParts)
{
    public UpdateMealCommand ToCommand(Guid mealId) => new(mealId, Name, Description, MealParts);
};