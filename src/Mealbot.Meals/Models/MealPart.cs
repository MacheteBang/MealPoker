using MealBot.Common.Enums;

namespace MealBot.Meals.Models;

public class MealPart(MealPartCategory category, string name, string? description, string? url) : ValueObject
{
    public required MealPartCategory Category { get; init; } = category;
    public required string Name { get; init; } = name;
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