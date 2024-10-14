namespace MealBot.Api.Meals.Features.GetMeal;

public record GetMealQuery(
    Guid MealId
) : IRequest<ErrorOr<Meal>>;