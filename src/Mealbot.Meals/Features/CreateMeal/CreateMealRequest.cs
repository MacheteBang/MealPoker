namespace Mealbot.Meals.Features.CreateMeal;

public record CreateMealRequest(
    string Name,
    string? Description)
{
    public CreateMealCommand ToCommand() => new(Name, Description);
};