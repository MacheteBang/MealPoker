namespace MealBot.Meals.Features.CreateMeal;

public class CreateMealCommandValidator : AbstractValidator<CreateMealCommand>
{
    public CreateMealCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Description)
            .MaximumLength(500);

        RuleFor(x => x.MealParts)
            .NotEmpty();

        RuleForEach(x => x.MealParts).ChildRules(mealPart =>
        {
            mealPart.RuleFor(y => y.Category)
                .IsInEnum()
                .NotEqual(MealPartCategory.Unknown);

            mealPart.RuleFor(x => x.Name)
                .NotEmpty();

            mealPart.RuleFor(x => x.Description)
                .MaximumLength(500);
        });

    }
}
