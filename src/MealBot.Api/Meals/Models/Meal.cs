namespace MealBot.Api.Meals.Models;

public class Meal
{
    public Guid MealId { get; init; } = Guid.NewGuid();
    public required Guid OwnerUserId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public ICollection<MealPart>? MealParts { get; set; } = [];

    public virtual User Owner { get; set; }

    public MealResponse ToResponse() => new(
        MealId,
        Owner.UserId,
        Owner.FirstName,
        Owner.LastName,
        Name,
        Description,
        MealParts is null ? null : [.. MealParts]
    );
}