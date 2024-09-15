namespace MealBot.Meals.Features.UpdateMeal;

public record UpdateMealCommand(
    Guid MealId,
    string Name,
    string? Description,
    List<MealPart> MealParts
) : IRequest<ErrorOr<Meal>>;