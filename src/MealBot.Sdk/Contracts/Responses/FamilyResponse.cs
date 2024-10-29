namespace MealBot.Sdk.Contracts.Responses;

public class FamilyResponse
{
    public required Guid FamilyId { get; init; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Code { get; set; }
}