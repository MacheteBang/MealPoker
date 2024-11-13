namespace MealBot.Sdk.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MealPartCategory
{
    MainCourse,
    Vegetable,
    SideDish,
    Dessert,
    Drink,
    Other
}