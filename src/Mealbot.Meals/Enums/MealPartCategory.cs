using System.Text.Json.Serialization;

namespace MealBot.Common.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MealPartCategory
    {
        MainCourse,
        Veggie,
        SideDish,
        Dessert,
        Drink,
        Other
    }
}