using MealBot.Meals.Models;

namespace MealBot.Meals.Interfaces;

public interface IMealRepository
{
    Task AddMeal(Meal meal);
    Task<List<Meal>> GetMeals();
    Task<Meal?> GetMeal(Guid mealId);
}