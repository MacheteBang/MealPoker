namespace MealBot.Api.Meals.Services;

public interface IMealsService
{
    Task<Meal?> GetMealByUserIdAsync(Guid ownerUserId, Guid mealId);
    Task<Meal?> UpdateMealAsync(Meal meal);
    Task<bool> DeleteMealByUserIdAsync(Guid ownerUserId, Guid mealId);
}

internal sealed class MealsService(MealBotDbContext mealBotDbContext) : IMealsService
{
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task<Meal?> GetMealByUserIdAsync(Guid ownerUserId, Guid mealId)
    {
        return await _mealBotDbContext.Meals
            .FirstOrDefaultAsync(m =>
                m.OwnerUserId == ownerUserId
                && m.MealId == mealId);
    }

    public async Task<Meal?> UpdateMealAsync(Meal meal)
    {
        var existingMeal = await GetMealByUserIdAsync(meal.OwnerUserId, meal.MealId);

        if (existingMeal is null)
        {
            return null;
        }

        _mealBotDbContext.Meals.Remove(existingMeal);
        await _mealBotDbContext.Meals.AddAsync(meal);
        await _mealBotDbContext.SaveChangesAsync();

        return meal;
    }

    public async Task<bool> DeleteMealByUserIdAsync(Guid ownerUserId, Guid mealId)
    {
        var existingMeal = await GetMealByUserIdAsync(ownerUserId, mealId);
        if (existingMeal is null)
        {
            return false;
        }

        _mealBotDbContext.Meals.Remove(existingMeal);
        await _mealBotDbContext.SaveChangesAsync();
        return true;
    }
}


