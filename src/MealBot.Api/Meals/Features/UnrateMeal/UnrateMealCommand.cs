namespace MealBot.Api.Meals.Features.UnrateMeal;

public sealed record UnrateMealCommand(
    Guid UserId,
    Guid MealId) : IRequest<ErrorOr<Success>>;