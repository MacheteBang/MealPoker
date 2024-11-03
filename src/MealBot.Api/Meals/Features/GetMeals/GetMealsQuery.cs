namespace MealBot.Api.Meals.Features.GetMeals;

public record GetMealsQuery(
    Guid OwnerUserId,
    bool IsFamilyMeals,
    bool IncludeCurrentUser
) : IRequest<ErrorOr<List<Meal>>>;