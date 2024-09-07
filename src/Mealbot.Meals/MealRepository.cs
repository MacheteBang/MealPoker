
namespace MealBot.Meals;

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

    public async Task<Meal?> UpdateMeal(Meal meal)
    {
        var existingMeal = _meals.FindIndex(m => m.MealId == meal.MealId);
        if (existingMeal == -1)
        {
            return null;
        }

        _meals[existingMeal] = meal;
        return meal;
    }

    public async Task<bool> DeleteMeal(Guid mealId)
    {
        var existingMeal = _meals.FindIndex(m => m.MealId == mealId);
        if (existingMeal == -1)
        {
            return false;
        }

        _meals.RemoveAt(existingMeal);
        return true;
    }
}