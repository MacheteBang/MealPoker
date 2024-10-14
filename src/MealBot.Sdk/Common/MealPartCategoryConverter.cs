namespace MealBot.Sdk.Common;

internal class MealPartCategoryConverter : JsonConverter<MealPartCategory>
{
    public override MealPartCategory Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var enumValue = reader.GetString();
            if (Enum.TryParse<MealPartCategory>(enumValue, true, out var category))
            {
                return category;
            }
        }
        return MealPartCategory.Unknown; // Default value for unknown categories
    }

    public override void Write(Utf8JsonWriter writer, MealPartCategory value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}