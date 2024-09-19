namespace MealBot.Meals.Features.GetMealPartCategories;

public record GetMealPartCategoriesQuery : IRequest<ErrorOr<List<MealPartCategory>>>;