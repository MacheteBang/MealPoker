namespace MealBot.Api.Meals.Features.GetMealRatings;
public record GetMealRatingsQuery : IRequest<ErrorOr<List<MealRating>>>;