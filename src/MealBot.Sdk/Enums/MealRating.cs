namespace MealBot.Sdk.Enums;

[JsonConverter(typeof(MealRatingConverter))]
public enum MealRating
{
    Unknown,
    Hate,
    Dislike,
    Neutral,
    Like,
    Love
}