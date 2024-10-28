namespace MealBot.Api.Meals.Features.CreateMeal;

public record CreateMealCommand(
    Guid OwnerUserId,
    string Name,
    string? Description,
    List<MealPart> MealParts
) : IRequest<ErrorOr<Meal>>;