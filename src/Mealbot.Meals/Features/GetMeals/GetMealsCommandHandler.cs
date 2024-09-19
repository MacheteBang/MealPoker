namespace MealBot.Meals.Features.GetMeals;

public sealed class GetMealsQueryHandler(IMealRepository mealRepository) : IRequestHandler<GetMealsQuery, ErrorOr<List<Meal>>>
{
    private readonly IMealRepository _mealRepository = mealRepository;

    public async Task<ErrorOr<List<Meal>>> Handle(GetMealsQuery request, CancellationToken cancellationToken)
    {
        var meals = await _mealRepository.GetMealsAsync();

        return meals;
    }
}