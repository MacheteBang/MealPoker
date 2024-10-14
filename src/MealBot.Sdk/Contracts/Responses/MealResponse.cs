namespace MealBot.Sdk.Contracts.Responses;

public class MealResponse
{
    public Guid MealId { get; init; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<MealPart> MealParts { get; set; } = [];
}
