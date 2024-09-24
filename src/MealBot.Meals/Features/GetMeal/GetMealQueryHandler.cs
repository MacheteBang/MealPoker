namespace MealBot.Meals.Features.GetMeal;

public sealed class GetMealQueryHandler(IMealRepository mealRepository) : IRequestHandler<GetMealQuery, ErrorOr<Meal>>
{
    private readonly IMealRepository _mealRepository = mealRepository;

    public async Task<ErrorOr<Meal>> Handle(GetMealQuery request, CancellationToken cancellationToken)
    {
        var meal = await _mealRepository.GetMealAsync(request.MealId);

        return meal switch
        {
            null => Errors.MealNotFoundError(request.MealId),
            _ => meal
        };
    }
}