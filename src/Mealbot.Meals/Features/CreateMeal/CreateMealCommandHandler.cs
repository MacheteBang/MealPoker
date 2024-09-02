namespace Mealbot.Meals.Features.CreateMeal;

public sealed class CreateMealCommandHandler(IMealRepository mealRepository) : IRequestHandler<CreateMealCommand, ErrorOr<Meal>>
{
    private readonly IMealRepository _mealRepository = mealRepository;

    public async Task<ErrorOr<Meal>> Handle(CreateMealCommand request, CancellationToken cancellationToken)
    {
        var meal = new Meal
        {
            Name = request.Name,
            Description = request.Description
        };

        await _mealRepository.AddMeal(meal);

        return meal;
    }
}