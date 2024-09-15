namespace MealBot.Meals.Interfaces;

public interface IMealRepository
{
    Task AddMeal(Meal meal);
    Task<List<Meal>> GetMealsAsync();
    Task<Meal?> GetMealAsync(Guid mealId);
    Task<Meal?> UpdateMeal(Meal meal);
    Task<bool> DeleteMeal(Guid mealId);
}