namespace MealBot.Api.Meals.Features.DeleteMeal;

public class DeleteMealCommandValidator : AbstractValidator<DeleteMealCommand>
{
    public DeleteMealCommandValidator()
    {
        RuleFor(x => x.OwnerUserId)
            .NotEmpty();

        RuleFor(x => x.MealId)
            .NotEmpty();
    }
}