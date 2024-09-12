using MealBot.Meals.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MealBot.Meals;

public sealed class MealRepository(MealsDbContext dbContext) : IMealRepository
{
    private readonly MealsDbContext _dbContext = dbContext;

    public async Task AddMeal(Meal meal)
    {
        _dbContext.Add(meal);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Meal>> GetMeals()
    {
        return await _dbContext.Meals.ToListAsync();
    }

    public async Task<Meal?> GetMeal(Guid mealId)
    {
        return await _dbContext.Meals.FirstOrDefaultAsync(meal => meal.MealId == mealId);
    }

    public async Task<Meal?> UpdateMeal(Meal meal)
    {
        var existingMeal = await _dbContext.Meals.FirstOrDefaultAsync(m => m.MealId == meal.MealId);
        if (existingMeal is null)
        {
            return null;
        }

        existingMeal = meal;
        await _dbContext.SaveChangesAsync();
        return meal;
    }

    public async Task<bool> DeleteMeal(Guid mealId)
    {
        var existingMeal = await _dbContext.Meals.FirstOrDefaultAsync(m => m.MealId == mealId);
        if (existingMeal is null)
        {
            return false;
        }

        _dbContext.Meals.Remove(existingMeal);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}