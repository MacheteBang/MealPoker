namespace MealBot.Api.Meals.Features.GetMeals;

public sealed class GetMealsQueryHandler(
    IValidator<GetMealsQuery> validator,
    IMealRepository mealRepository) : IRequestHandler<GetMealsQuery, ErrorOr<List<Meal>>>
{
    private readonly IValidator<GetMealsQuery> _validator = validator;
    private readonly IMealRepository _mealRepository = mealRepository;

    public async Task<ErrorOr<List<Meal>>> Handle(GetMealsQuery query, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(query);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        var meals = await _mealRepository.GetMealsByUserIdAsync(query.OwnerUserId);

        return meals;
    }
}