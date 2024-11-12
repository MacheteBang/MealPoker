namespace MealBot.Api.Meals.Features.RateMeal;

public class RateMealCommandValidator : AbstractValidator<RateMealCommand>
{
    public RateMealCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.MealId).NotEmpty();
        RuleFor(x => x.Rating).NotEqual(MealRating.Unknown);
    }
}