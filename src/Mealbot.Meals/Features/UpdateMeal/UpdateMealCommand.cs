namespace MealBot.Meals.Features.UpdateMeal;

public record UpdateMealCommand(
    Guid MealId,
    string Name,
    string? Description
) : IRequest<ErrorOr<Meal>>;