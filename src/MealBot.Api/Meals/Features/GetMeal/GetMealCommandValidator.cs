namespace MealBot.Api.Meals.Features.GetMeal;

public class GetMealCommandValidator : AbstractValidator<GetMealQuery>
{
    public GetMealCommandValidator()
    {
        RuleFor(x => x.OwnerUserId)
            .NotEmpty();

        RuleFor(x => x.MealId)
            .NotEmpty();
    }
}