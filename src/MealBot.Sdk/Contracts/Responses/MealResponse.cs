namespace MealBot.Sdk.Contracts.Responses;

public record MealResponse(
    Guid MealId,
    Guid OwnerUserId,
    string OwnerFirstName,
    string OwnerLastName,
    string Name,
    string? Description,
    List<MealPart>? MealParts
);