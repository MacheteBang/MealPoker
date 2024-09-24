namespace MealBot.Meals.ValueObjects;

public class MealPart(MealPartCategory category, string name, string? description, string? url) : ValueObject
{
    public MealPartCategory Category { get; init; } = category;
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