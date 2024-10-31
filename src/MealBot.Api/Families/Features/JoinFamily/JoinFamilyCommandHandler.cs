namespace MealBot.Api.Families.Features.JoinFamily;

internal sealed class JoinFamilyCommandHandler(
    IValidator<JoinFamilyCommand> validator,
    MealBotDbContext mealBotDbContext) : IRequestHandler<JoinFamilyCommand, ErrorOr<Success>>
{
    private readonly IValidator<JoinFamilyCommand> _validator = validator;
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task<ErrorOr<Success>> Handle(JoinFamilyCommand command, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(command);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        var family = await _mealBotDbContext.Families
            .SingleOrDefaultAsync(x =>
                x.FamilyId == command.FamilyId
                && x.Code == command.Code, cancellationToken);

        if (family is null)
        {
            return Errors.InvalidCode();
        }

        var user = await _mealBotDbContext.Users
            .SingleOrDefaultAsync(x => x.UserId == command.UserId, cancellationToken);

        if (user is null)
        {
            return Users.Errors.UserNotFound();
        }

        if (user.FamilyId.HasValue)
        {
            return Errors.UserAlreadyHasFamily();
        }

        user.FamilyId = family.FamilyId;
        _mealBotDbContext.Update(user);
        await _mealBotDbContext.SaveChangesAsync(cancellationToken);

        return new Success();
    }
}