namespace Mealbot.Meals.Features.CreateMeal;

public record CreateMealCommand(
    string Name,
    string? Description
) : IRequest<ErrorOr<Meal>>;