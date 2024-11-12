namespace MealBot.Sdk.Common;

internal class MealRatingConverter : JsonConverter<MealRating>
{
    public override MealRating Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var enumValue = reader.GetString();
            if (Enum.TryParse<MealRating>(enumValue, true, out var category))
            {
                return category;
            }
        }
        return MealRating.Unknown; // Default value for unknown categories
    }

    public override void Write(Utf8JsonWriter writer, MealRating value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}