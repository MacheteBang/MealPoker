namespace MealBot.Api.Meals.Features.RateMeal;

public sealed record RateMealCommand(
    Guid UserId,
    Guid MealId,
    MealRating Rating) : IRequest<ErrorOr<Success>>;