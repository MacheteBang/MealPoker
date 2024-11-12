namespace MealBot.Api.Meals.Features.GetMeal;

internal sealed class GetMealQueryHandler(
    IValidator<GetMealQuery> validator,
    MealBotDbContext mealBotDbContext) : IRequestHandler<GetMealQuery, ErrorOr<Meal>>
{
    private readonly IValidator<GetMealQuery> _validator = validator;
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task<ErrorOr<Meal>> Handle(GetMealQuery query, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(query);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        var meal = await _mealBotDbContext.Meals
            .Include(m => m.Owner)
            .Include(m => m.Ratings)
            .FirstOrDefaultAsync(m =>
                m.OwnerUserId == query.OwnerUserId
                && m.MealId == query.MealId,
                cancellationToken);

        return meal switch
        {
            null => Errors.MealNotFoundError(query.MealId),
            _ => meal
        };
    }
}