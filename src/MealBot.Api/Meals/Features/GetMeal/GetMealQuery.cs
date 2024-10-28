namespace MealBot.Api.Meals.Features.GetMeal;

public record GetMealQuery(
    Guid OwnerUserId,
    Guid MealId
) : IRequest<ErrorOr<Meal>>;