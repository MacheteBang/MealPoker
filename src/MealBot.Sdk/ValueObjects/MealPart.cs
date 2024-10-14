namespace MealBot.Sdk.ValueObjects;

public class MealPart(string category, string name, string? description, string? url) : ValueObject
{
    public string Category { get; init; } = category;
    public string Name { get; init; } = name;
    public string? Description { get; } = description;
    public string? Url { get; } = url;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Category;
        yield return Name;
        yield return Description ?? string.Empty;
        yield return Url ?? string.Empty;
    }
}