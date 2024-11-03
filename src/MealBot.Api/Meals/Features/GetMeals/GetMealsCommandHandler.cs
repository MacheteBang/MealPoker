namespace MealBot.Api.Meals.Features.GetMeals;

internal sealed class GetMealsQueryHandler(
    IValidator<GetMealsQuery> validator,
    MealBotDbContext mealBotDbContext) : IRequestHandler<GetMealsQuery, ErrorOr<List<Meal>>>
{
    private readonly IValidator<GetMealsQuery> _validator = validator;
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task<ErrorOr<List<Meal>>> Handle(GetMealsQuery query, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(query);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        if (!query.IsFamilyMeals) // User's meals only
        {
            return await _mealBotDbContext.Meals
                .Include(m => m.Owner)
                .Where(m => m.OwnerUserId == query.OwnerUserId)
                .ToListAsync(cancellationToken);
        }

        // Family's meals
        var user = await _mealBotDbContext.Users.SingleOrDefaultAsync(u => u.UserId == query.OwnerUserId);
        if (user is null)
        {
            return Users.Errors.UserNotFound();
        }

        var meals = await _mealBotDbContext.Meals
            .Include(m => m.Owner)
            .Where(m => m.Owner.FamilyId == user.FamilyId)
            .Where(m => query.IncludeCurrentUser || m.OwnerUserId != query.OwnerUserId)
            .ToListAsync(cancellationToken);

        return meals;
    }
}