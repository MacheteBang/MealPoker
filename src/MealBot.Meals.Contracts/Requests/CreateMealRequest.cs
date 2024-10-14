namespace MealBot.Meals.Contracts.Requests;

public record CreateMealRequest(
    string Name,
    string? Description,
    List<MealPart> MealParts);