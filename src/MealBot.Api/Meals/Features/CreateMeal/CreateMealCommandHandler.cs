namespace MealBot.Api.Meals.Features.CreateMeal;

internal sealed class CreateMealCommandHandler(
    IValidator<CreateMealCommand> validator,
    MealBotDbContext mealBotDbContext) : IRequestHandler<CreateMealCommand, ErrorOr<Meal>>
{
    private readonly IValidator<CreateMealCommand> _validator = validator;
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

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

        await _mealBotDbContext.Meals.AddAsync(meal);
        await _mealBotDbContext.SaveChangesAsync();

        return meal;
    }
}