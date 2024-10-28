namespace MealBot.Sdk.Contracts.Requests;

public record UpdateMealRequest(
    string Name,
    string? Description,
    List<MealPart> MealParts);