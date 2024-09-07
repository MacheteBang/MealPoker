namespace MealBot.Meals.Features.UpdateMeal;

public record UpdateMealRequest(
    string Name,
    string? Description)
{
    public UpdateMealCommand ToCommand(Guid mealId) => new(mealId, Name, Description);
};