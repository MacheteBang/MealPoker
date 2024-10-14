namespace MealBot.Api.Meals.Features.GetMeals;

public record GetMealsQuery : IRequest<ErrorOr<List<Meal>>>;