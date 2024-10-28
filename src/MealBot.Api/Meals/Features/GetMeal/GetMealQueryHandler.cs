namespace MealBot.Api.Meals.Features.GetMeal;

public sealed class GetMealQueryHandler(
    IValidator<GetMealQuery> validator,
    IMealRepository mealRepository) : IRequestHandler<GetMealQuery, ErrorOr<Meal>>
{
    private readonly IValidator<GetMealQuery> _validator = validator;
    private readonly IMealRepository _mealRepository = mealRepository;

    public async Task<ErrorOr<Meal>> Handle(GetMealQuery query, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(query);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        var meal = await _mealRepository.GetMealByUserIdAsync(query.OwnerUserId, query.MealId);

        return meal switch
        {
            null => Errors.MealNotFoundError(query.MealId),
            _ => meal
        };
    }
}