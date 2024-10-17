namespace MealBot.Api.Meals.Repositories;

public interface IMealRepository
{
    Task AddMealAsync(Meal meal);
    Task<List<Meal>> GetMealsAsync();
    Task<Meal?> GetMealAsync(Guid mealId);
    Task<Meal?> UpdateMeal(Meal meal);
    Task<bool> DeleteMeal(Guid mealId);
}

internal sealed class MealRepository(MealBotDbContext mealBotDbContext) : IMealRepository
{
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task AddMealAsync(Meal meal)
    {
        await _mealBotDbContext.Meals.AddAsync(meal);
        await _mealBotDbContext.SaveChangesAsync();
    }

    public Task<List<Meal>> GetMealsAsync()
    {
        return Task.FromResult<List<Meal>>([.. _mealBotDbContext.Meals]);
    }

    public Task<Meal?> GetMealAsync(Guid mealId)
    {
        return Task.FromResult(_mealBotDbContext.Meals.FirstOrDefault(m => m.MealId == mealId));
    }

    public async Task<Meal?> UpdateMeal(Meal meal)
    {
        var existingMeal = _mealBotDbContext.Meals.FirstOrDefault(m => m.MealId == meal.MealId);
        if (existingMeal is null)
        {
            return null;
        }

        _mealBotDbContext.Meals.Remove(existingMeal);
        await _mealBotDbContext.Meals.AddAsync(meal);
        await _mealBotDbContext.SaveChangesAsync();

        return meal;
    }

    public async Task<bool> DeleteMeal(Guid mealId)
    {
        var existingMeal = _mealBotDbContext.Meals.FirstOrDefault(m => m.MealId == mealId);
        if (existingMeal is null)
        {
            return false;
        }

        _mealBotDbContext.Meals.Remove(existingMeal);
        await _mealBotDbContext.SaveChangesAsync();
        return true;
    }
}


