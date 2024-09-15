namespace MealBot.Meals.Features.CreateMeal;

public record CreateMealCommand(
    string Name,
    string? Description,
    List<MealPart> MealParts
) : IRequest<ErrorOr<Meal>>;