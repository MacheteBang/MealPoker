namespace MealBot.Meals.Models;

public class Meal
{
    public Guid MealId { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<MealPart> MealParts { get; set; } = [];
}