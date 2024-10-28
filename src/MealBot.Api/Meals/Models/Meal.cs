namespace MealBot.Api.Meals.Models;

public class Meal
{
    public Guid MealId { get; init; } = Guid.NewGuid();
    public required Guid OwnerUserId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<MealPart>? MealParts { get; set; } = [];

    public MealResponse ToResponse() =>
        new()
        {
            MealId = MealId,
            Name = Name,
            Description = Description,
            MealParts = MealParts is null
                ? null
                : [.. MealParts]
        };
}