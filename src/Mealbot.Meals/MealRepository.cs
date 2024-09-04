
namespace Mealbot.Meals;

public sealed class MealRepository : IMealRepository
{
    private readonly List<Meal> _meals = [];

    public async Task AddMeal(Meal meal)
    {
        _meals.Add(meal);
    }

    public async Task<List<Meal>> GetMeals()
    {
        return [.. _meals];
    }

    public async Task<Meal?> GetMeal(Guid mealId)
    {
        return _meals.Find(meal => meal.MealId == mealId);
    }
}