namespace MealBot.Sdk.ValueObjects;

public class MealPart(MealPartCategory category, string name, string? description, string? url) : ValueObject
{
    // Parameterless constructor for EF Core
    public MealPart() : this(MealPartCategory.Other, string.Empty, null, null) { }

    public MealPartCategory Category { get; init; } = category;
    public string Name { get; init; } = name;
    public string? Description { get; init; } = description;
    public string? Url { get; init; } = url;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Category;
        yield return Name;
        yield return Description ?? string.Empty;
        yield return Url ?? string.Empty;
    }
}