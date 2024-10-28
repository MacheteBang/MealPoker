namespace MealBot.Api.Meals.Features.UpdateMeal;

public class UpdateMealCommandValidator : AbstractValidator<UpdateMealCommand>
{
    public UpdateMealCommandValidator()
    {
        RuleFor(x => x.OwnerUserId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.MealParts)
            .NotEmpty();

        RuleForEach(x => x.MealParts)
            .SetValidator(new MealPartValidator());
    }
}