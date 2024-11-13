
namespace MealBot.Api.Meals.Features.UnrateMeal;

internal sealed class UnrateMealCommandHandler(
    IValidator<UnrateMealCommand> validator,
    MealBotDbContext mealBotDbContext) : IRequestHandler<UnrateMealCommand, ErrorOr<Success>>
{
    private readonly IValidator<UnrateMealCommand> _validator = validator;
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task<ErrorOr<Success>> Handle(UnrateMealCommand command, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        // Family's meals
        var user = await _mealBotDbContext.Users.SingleOrDefaultAsync(u => u.UserId == command.UserId);
        if (user is null)
        {
            return Users.Errors.UserNotFound();
        }

        await _mealBotDbContext.UserMealRatings
            .Where(r => r.UserId == command.UserId && r.MealId == command.MealId)
            .ExecuteDeleteAsync(cancellationToken);

        return new Success();
    }
}