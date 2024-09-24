using System.Text.Json.Serialization;

namespace MealBot.Meals.Enums;

[JsonConverter(typeof(MealPartCategoryConverter))]
public enum MealPartCategory
{
    Unknown,
    MainCourse,
    Veggie,
    SideDish,
    Dessert,
    Drink,
    Other
}