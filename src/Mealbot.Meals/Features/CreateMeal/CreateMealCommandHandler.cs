namespace MealBot.Meals.Features.CreateMeal;

public sealed class CreateMealCommandHandler(IValidator<CreateMealCommand> validator, IMealRepository mealRepository) : IRequestHandler<CreateMealCommand, ErrorOr<Meal>>
{
    private readonly IValidator<CreateMealCommand> _validator = validator;
    private readonly IMealRepository _mealRepository = mealRepository;

    public async Task<ErrorOr<Meal>> Handle(CreateMealCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = _validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return validationResult.Errors
                .Select(x => Errors.InvalidRequest(x.ErrorMessage))
                .ToList();
        }

        var meal = new Meal
        {
            Name = request.Name,
            Description = request.Description,
            MealParts = request.MealParts
        };

        await _mealRepository.AddMeal(meal);

        return meal;
    }
}