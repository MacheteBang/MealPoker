namespace MealBot.Meals.Features.DeleteMeal;

public sealed class DeleteMealCommandHandler(IMealRepository mealRepository) : IRequestHandler<DeleteMealCommand, ErrorOr<Success>>
{
    private readonly IMealRepository _mealRepository = mealRepository;

    public async Task<ErrorOr<Success>> Handle(DeleteMealCommand request, CancellationToken cancellationToken)
    {
        var meal = await _mealRepository.DeleteMeal(request.MealId);

        if (!meal)
        {
            return Errors.MealNotFoundError(request.MealId);
        }

        return Result.Success;
    }
}