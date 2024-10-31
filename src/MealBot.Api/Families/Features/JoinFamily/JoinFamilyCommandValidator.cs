namespace MealBot.Api.Families.Features.JoinFamily;

public class JoinFamilyCommandValidator : AbstractValidator<JoinFamilyCommand>
{
    public JoinFamilyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Code).NotEmpty();
    }
}