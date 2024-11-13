namespace MealBot.Api.Meals.Features.UnrateMeal;

public class UnrateMealCommandValidator : AbstractValidator<UnrateMealCommand>
{
    public UnrateMealCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.MealId).NotEmpty();
    }
}