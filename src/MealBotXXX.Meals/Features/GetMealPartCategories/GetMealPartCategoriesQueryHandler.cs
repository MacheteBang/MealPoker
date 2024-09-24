namespace MealBot.Meals.Features.GetMealPartCategories;

public sealed class GetMealPartCategoriesQueryHandler() : IRequestHandler<GetMealPartCategoriesQuery, ErrorOr<List<MealPartCategory>>>
{
    public Task<ErrorOr<List<MealPartCategory>>> Handle(GetMealPartCategoriesQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(Enum.GetValues<MealPartCategory>().ToList().ToErrorOr());
    }
}