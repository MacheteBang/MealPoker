namespace MealBot.Api.Families.Features.LeaveFamily;

internal sealed class LeaveFamilyCommandHandler(IValidator<LeaveFamilyCommand> validator, MealBotDbContext mealBotDbContext) : IRequestHandler<LeaveFamilyCommand, ErrorOr<Success>>
{
    private readonly IValidator<LeaveFamilyCommand> _validator = validator;
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task<ErrorOr<Success>> Handle(LeaveFamilyCommand command, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        var user = await _mealBotDbContext.Users
            .SingleOrDefaultAsync(x => x.UserId == command.UserId, cancellationToken);

        if (user is null)
        {
            return Users.Errors.UserNotFound();
        }

        if (user.FamilyId != command.FamilyId)
        {
            return Errors.FamilyNotFound();
        }

        user.FamilyId = null;
        _mealBotDbContext.Users.Update(user);
        await _mealBotDbContext.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}