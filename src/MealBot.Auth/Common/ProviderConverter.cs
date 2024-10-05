namespace MealBot.Auth.Common;

public class ProviderConverter : JsonConverter<Provider>
{
    public override Provider Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var enumValue = reader.GetString();
            if (Enum.TryParse<Provider>(enumValue, true, out var category))
            {
                return category;
            }
        }
        return Provider.Unknown; // Default value for unknown categories
    }

    public override void Write(Utf8JsonWriter writer, Provider value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}