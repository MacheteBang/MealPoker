namespace MealBot.Api.Meals.Features.GetMealPartCategories;

public record GetMealPartCategoriesQuery : IRequest<ErrorOr<List<MealPartCategory>>>;