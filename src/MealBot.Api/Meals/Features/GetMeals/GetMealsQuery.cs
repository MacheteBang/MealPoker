namespace MealBot.Api.Meals.Features.GetMeals;

public record GetMealsQuery(
    Guid OwnerUserId
) : IRequest<ErrorOr<List<Meal>>>;