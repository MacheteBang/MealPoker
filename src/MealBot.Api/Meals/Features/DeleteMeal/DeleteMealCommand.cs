namespace MealBot.Api.Meals.Features.DeleteMeal;

public record DeleteMealCommand(
    Guid OwnerUserId,
    Guid MealId
) : IRequest<ErrorOr<Success>>;