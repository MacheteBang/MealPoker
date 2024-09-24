namespace MealBot.Meals.Features.GetMeal;

public record GetMealQuery(
    Guid MealId
) : IRequest<ErrorOr<Meal>>;