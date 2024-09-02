namespace Mealbot.Meals.Models;

public class Meal
{
    public Guid MealId { get; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? Description { get; set; }
}