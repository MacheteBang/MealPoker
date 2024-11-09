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

        var user = await _mealBotDbContext.Users.FindAsync(command.OwnerUserId, cancellationToken);
        if (user == null)
        {
            return Users.Errors.UserNotFound();
        }

        var meal = new Meal
        {
            OwnerUserId = command.OwnerUserId,
            Owner = user,
            Name = command.Name,
            Description = command.Description,
            MealParts = command.MealParts
        };

        await _mealBotDbContext.Meals.AddAsync(meal, cancellationToken);
        await _mealBotDbContext.SaveChangesAsync(cancellationToken);

        return meal;
    }
}