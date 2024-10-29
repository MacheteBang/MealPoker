namespace MealBot.Api.Meals.Features.CreateMeal;

public class CreateMealCommandValidator : AbstractValidator<CreateMealCommand>
{
    public CreateMealCommandValidator()
    {
        RuleFor(x => x.OwnerUserId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleForEach(x => x.MealParts)
            .SetValidator(new MealPartValidator());
    }
}