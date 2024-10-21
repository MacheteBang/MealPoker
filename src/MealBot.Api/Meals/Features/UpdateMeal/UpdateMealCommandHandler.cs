namespace MealBot.Api.Meals.Features.UpdateMeal;

public sealed class UpdateMealCommandHandler(IMealRepository mealRepository) : IRequestHandler<UpdateMealCommand, ErrorOr<Meal>>
{
    private readonly IMealRepository _mealRepository = mealRepository;

    public async Task<ErrorOr<Meal>> Handle(UpdateMealCommand request, CancellationToken cancellationToken)
    {
        var meal = new Meal
        {
            MealId = request.MealId,
            Name = request.Name,
            Description = request.Description,
            MealParts = request.MealParts
        };

        var updatedMeal = await _mealRepository.UpdateMeal(meal);

        return updatedMeal switch
        {
            null => Errors.MealNotFoundError(request.MealId),
            _ => updatedMeal!
        };
    }
}