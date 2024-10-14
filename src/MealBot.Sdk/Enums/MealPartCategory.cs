namespace MealBot.Sdk.Enums;

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