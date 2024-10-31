namespace MealBot.Api.Families.Features.GetFamily;

internal sealed class GetFamilyQueryHandler(IValidator<GetFamilyQuery> validator, MealBotDbContext mealBotDbContext) : IRequestHandler<GetFamilyQuery, ErrorOr<Family>>
{
    private readonly IValidator<GetFamilyQuery> _validator = validator;
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task<ErrorOr<Family>> Handle(GetFamilyQuery query, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(query);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest($"{x.FormattedMessagePlaceholderValues.SingleOrDefault(c => c.Key == "PropertyPath").Value}: {x.ErrorMessage}"))
                .ToList();
        }

        var family = await _mealBotDbContext.Families
            .Include(x => x.User)
            .SingleOrDefaultAsync(x =>
                x.FamilyId == query.FamilyId
                && x.User != null
                && x.User.Select(u => u.UserId).Contains(query.UserId), cancellationToken);

        if (family is null)
        {
            return Errors.FamilyNotFound();
        }

        return family;
    }
}