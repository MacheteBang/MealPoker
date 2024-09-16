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
            .NotEmpty()
            .ForEach(x => x.SetValidator(new MealPartValidator()));
    }
}
