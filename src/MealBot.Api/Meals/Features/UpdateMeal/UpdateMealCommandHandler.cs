namespace MealBot.Api.Meals.Features.UpdateMeal;

public sealed class UpdateMealCommandHandler(
    IValidator<UpdateMealCommand> validator,
    IMealRepository mealRepository) : IRequestHandler<UpdateMealCommand, ErrorOr<Meal>>
{
    private readonly IValidator<UpdateMealCommand> _validator = validator;
    private readonly IMealRepository _mealRepository = mealRepository;

    public async Task<ErrorOr<Meal>> Handle(UpdateMealCommand command, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        var meal = new Meal
        {
            OwnerUserId = command.OwnerUserId,
            MealId = command.MealId,
            Name = command.Name,
            Description = command.Description,
            MealParts = command.MealParts
        };

        var updatedMeal = await _mealRepository.UpdateMealAsync(meal);

        return updatedMeal switch
        {
            null => Errors.MealNotFoundError(command.MealId),
            _ => updatedMeal!
        };
    }
}