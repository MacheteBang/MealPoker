namespace MealBot.Api.Meals.Features.GetMeals;

public class GetMealsCommandValidator : AbstractValidator<GetMealsQuery>
{
    public GetMealsCommandValidator()
    {
        RuleFor(x => x.OwnerUserId)
            .NotEmpty();
    }
}