namespace MealBot.Api.Families.Features.GetFamily;

public class GetFamilyQueryValidator : AbstractValidator<GetFamilyQuery>
{
    public GetFamilyQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FamilyId).NotEmpty();
    }
}
