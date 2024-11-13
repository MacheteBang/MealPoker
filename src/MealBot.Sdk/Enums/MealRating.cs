namespace MealBot.Sdk.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MealRating
{
    Hate,
    Dislike,
    Neutral,
    Like,
    Love
}