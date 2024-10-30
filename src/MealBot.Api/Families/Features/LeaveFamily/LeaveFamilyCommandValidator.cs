namespace MealBot.Api.Families.Features.LeaveFamily;

public class LeaveFamilyCommandValidator : AbstractValidator<LeaveFamilyCommand>
{
    public LeaveFamilyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.FamilyId).NotEmpty();
    }
}
