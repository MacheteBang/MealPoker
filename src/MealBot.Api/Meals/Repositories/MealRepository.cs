using Microsoft.EntityFrameworkCore;

namespace MealBot.Api.Meals.Repositories;

public interface IMealRepository
{
    Task AddMealAsync(Meal meal);
    Task<List<Meal>> GetMealsByUserIdAsync(Guid ownerUserId);
    Task<Meal?> GetMealByUserIdAsync(Guid ownerUserId, Guid mealId);
    Task<Meal?> UpdateMealAsync(Meal meal);
    Task<bool> DeleteMealByUserIdAsync(Guid ownerUserId, Guid mealId);
}

internal sealed class MealRepository(MealBotDbContext mealBotDbContext) : IMealRepository
{
    private readonly MealBotDbContext _mealBotDbContext = mealBotDbContext;

    public async Task AddMealAsync(Meal meal)
    {
        await _mealBotDbContext.Meals.AddAsync(meal);
        await _mealBotDbContext.SaveChangesAsync();
    }

    public async Task<List<Meal>> GetMealsByUserIdAsync(Guid ownerUserId)
    {
        return await _mealBotDbContext.Meals
            .Where(m => m.OwnerUserId == ownerUserId)
            .ToListAsync();
    }

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


