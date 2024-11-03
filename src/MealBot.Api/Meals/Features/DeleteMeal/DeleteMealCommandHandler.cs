namespace MealBot.Api.Meals.Features.DeleteMeal;

internal sealed class DeleteMealCommandHandler(
    IValidator<DeleteMealCommand> validator,
    MealBotDbContext mealBotDbContext) : IRequestHandler<DeleteMealCommand, ErrorOr<Success>>
{
    private readonly IValidator<DeleteMealCommand> _validator = validator;
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task<ErrorOr<Success>> Handle(DeleteMealCommand command, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        var meal = await _mealBotDbContext.Meals
            .FirstOrDefaultAsync(m =>
                m.OwnerUserId == command.OwnerUserId
                && m.MealId == command.MealId);

        if (meal is null)
        {
            return Errors.MealNotFoundError(command.MealId);
        }

        _mealBotDbContext.Meals.Remove(meal);
        await _mealBotDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success;
    }
}