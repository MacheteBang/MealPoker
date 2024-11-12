namespace MealBot.Api.Meals.Models;

public class UserMealRating
{
    public required Guid MealId { get; set; }
    public required Guid UserId { get; set; }
    public required MealRating Rating { get; set; }

    public virtual Meal Meal { get; set; }
    public virtual User User { get; set; }
}