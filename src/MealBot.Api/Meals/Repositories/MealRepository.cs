namespace MealBot.Api.Meals.Repositories;

public interface IMealRepository
{
    Task AddMeal(Meal meal);
    Task<List<Meal>> GetMealsAsync();
    Task<Meal?> GetMealAsync(Guid mealId);
    Task<Meal?> UpdateMeal(Meal meal);
    Task<bool> DeleteMeal(Guid mealId);
}

public sealed class MealRepository : IMealRepository
{
    private readonly List<Meal> _meals = [];

    public Task AddMeal(Meal meal)
    {
        _meals.Add(meal);
        return Task.CompletedTask;
    }

    public Task<List<Meal>> GetMealsAsync()
    {
        return Task.FromResult(_meals);
    }

    public Task<Meal?> GetMealAsync(Guid mealId)
    {
        return Task.FromResult(_meals.FirstOrDefault(m => m.MealId == mealId));
    }

    public Task<Meal?> UpdateMeal(Meal meal)
    {
        var existingMeal = _meals.FirstOrDefault(m => m.MealId == meal.MealId);
        if (existingMeal is null)
        {
            return Task.FromResult<Meal?>(null);
        }

        _meals.Remove(existingMeal);
        _meals.Add(meal);

        return Task.FromResult<Meal?>(meal);
    }

    public Task<bool> DeleteMeal(Guid mealId)
    {
        var existingMeal = _meals.FirstOrDefault(m => m.MealId == mealId);
        if (existingMeal is null)
        {
            return Task.FromResult(false);
        }

        _meals.Remove(existingMeal);
        return Task.FromResult(true);
    }
}


