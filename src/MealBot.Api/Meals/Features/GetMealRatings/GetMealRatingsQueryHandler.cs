namespace MealBot.Api.Meals.Features.GetMealRatings;

public sealed class GetMealPartCategoriesQueryHandler() : IRequestHandler<GetMealRatingsQuery, ErrorOr<List<MealRating>>>
{
    public Task<ErrorOr<List<MealRating>>> Handle(GetMealRatingsQuery query, CancellationToken cancellationToken)
    {
        return Task.FromResult(Enum.GetValues<MealRating>().ToList().ToErrorOr());
    }
}