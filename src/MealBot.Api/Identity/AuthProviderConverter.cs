namespace MealBot.Api.Identity;

internal class AuthProviderConverter : JsonConverter<AuthProvider>
{
    public override AuthProvider Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var enumValue = reader.GetString();
            if (Enum.TryParse<AuthProvider>(enumValue, true, out var category))
            {
                return category;
            }
        }
        return AuthProvider.Unknown; // Default value for unknown categories
    }

    public override void Write(Utf8JsonWriter writer, AuthProvider value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}