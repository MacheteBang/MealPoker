namespace MealBot.Api.Meals.Features.UpdateMeal;

internal sealed class UpdateMealCommandHandler(
    IValidator<UpdateMealCommand> validator,
    MealBotDbContext mealBotDbContext) : IRequestHandler<UpdateMealCommand, ErrorOr<Meal>>
{
    private readonly IValidator<UpdateMealCommand> _validator = validator;
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task<ErrorOr<Meal>> Handle(UpdateMealCommand command, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        var existingMeal = await _mealBotDbContext.Meals
            .FirstOrDefaultAsync(m =>
                m.OwnerUserId == command.OwnerUserId
                && m.MealId == command.MealId);

        if (existingMeal is null)
        {
            return Errors.MealNotFoundError(command.MealId);
        }

        var meal = new Meal
        {
            OwnerUserId = command.OwnerUserId,
            MealId = command.MealId,
            Name = command.Name,
            Description = command.Description,
            MealParts = command.MealParts
        };

        _mealBotDbContext.Meals.Remove(existingMeal);
        await _mealBotDbContext.Meals.AddAsync(meal);
        await _mealBotDbContext.SaveChangesAsync();

        return meal;
    }
}