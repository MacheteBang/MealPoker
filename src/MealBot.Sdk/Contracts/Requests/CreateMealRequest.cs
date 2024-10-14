namespace MealBot.Sdk.Contracts.Requests;

public record CreateMealRequest(
    string Name,
    string? Description,
    List<MealPart> MealParts);