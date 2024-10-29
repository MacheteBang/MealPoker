namespace MealBot.Api.Meals.Features.CreateMeal;

public sealed class CreateMealCommandHandler(IValidator<CreateMealCommand> validator, IMealsService MealsService) : IRequestHandler<CreateMealCommand, ErrorOr<Meal>>
{
    private readonly IValidator<CreateMealCommand> _validator = validator;
    private readonly IMealsService _MealsService = MealsService;

    public async Task<ErrorOr<Meal>> Handle(CreateMealCommand command, CancellationToken cancellationToken)
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
            Name = command.Name,
            Description = command.Description,
            MealParts = command.MealParts
        };

        await _MealsService.AddMealAsync(meal);

        return meal;
    }
}