namespace MealBot.Sdk.Enums;

[JsonConverter(typeof(MealPartCategoryConverter))]
public enum MealPartCategory
{
    Unknown,
    MainCourse,
    Vegetable,
    SideDish,
    Dessert,
    Drink,
    Other
}