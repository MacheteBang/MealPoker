namespace MealBot.Api.Meals.Features.UpdateMeal;

public record UpdateMealCommand(
    Guid OwnerUserId,
    Guid MealId,
    string Name,
    string? Description,
    List<MealPart> MealParts
) : IRequest<ErrorOr<Meal>>;