
namespace MealBot.Api.Meals.Features.RateMeal;

internal sealed class RateMealCommandHandler(
    IValidator<RateMealCommand> validator,
    MealBotDbContext mealBotDbContext) : IRequestHandler<RateMealCommand, ErrorOr<Success>>
{
    private readonly IValidator<RateMealCommand> _validator = validator;
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task<ErrorOr<Success>> Handle(RateMealCommand command, CancellationToken cancellationToken)
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

        var mealExists = await _mealBotDbContext.Meals
            .Include(m => m.Owner)
            .AnyAsync(m =>
                m.Owner.FamilyId == user.FamilyId || m.OwnerUserId == command.UserId,
                cancellationToken);

        if (!mealExists)
        {
            return Errors.MealNotFoundError(command.MealId);
        }

        var existingUserMealRating = await _mealBotDbContext.UserMealRatings
            .FirstOrDefaultAsync(m =>
                m.UserId == command.UserId && m.MealId == command.MealId,
                cancellationToken);

        if (existingUserMealRating is not null)
        {
            existingUserMealRating.Rating = command.Rating;
            _mealBotDbContext.UserMealRatings.Update(existingUserMealRating);
            await _mealBotDbContext.SaveChangesAsync(cancellationToken);
            return new Success();
        }

        var newUserMealRating = new UserMealRating
        {
            UserId = command.UserId,
            MealId = command.MealId,
            Rating = command.Rating
        };

        _mealBotDbContext.UserMealRatings.Add(newUserMealRating);
        await _mealBotDbContext.SaveChangesAsync(cancellationToken);
        return new Success();
    }
}