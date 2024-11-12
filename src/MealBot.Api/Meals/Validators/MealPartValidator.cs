namespace MealBot.Api.Meals.Validators;

public class MealPartValidator : AbstractValidator<MealPart>
{
    public MealPartValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).MaximumLength(500);
    }
}
