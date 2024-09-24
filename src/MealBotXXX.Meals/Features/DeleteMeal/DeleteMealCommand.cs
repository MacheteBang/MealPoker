namespace MealBot.Meals.Features.DeleteMeal;

public record DeleteMealCommand(
    Guid MealId
) : IRequest<ErrorOr<Success>>;