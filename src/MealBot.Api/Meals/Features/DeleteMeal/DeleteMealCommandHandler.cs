namespace MealBot.Api.Meals.Features.DeleteMeal;

public sealed class DeleteMealCommandHandler(IValidator<DeleteMealCommand> validator, IMealRepository mealRepository) : IRequestHandler<DeleteMealCommand, ErrorOr<Success>>
{
    private readonly IValidator<DeleteMealCommand> _validator = validator;
    private readonly IMealRepository _mealRepository = mealRepository;

    public async Task<ErrorOr<Success>> Handle(DeleteMealCommand command, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        var meal = await _mealRepository.DeleteMealByUserIdAsync(command.OwnerUserId, command.MealId);

        if (!meal)
        {
            return Errors.MealNotFoundError(command.MealId);
        }

        return Result.Success;
    }
}