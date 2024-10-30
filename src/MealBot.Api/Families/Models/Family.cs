namespace MealBot.Api.Families.Models;

internal class Family
{
    public Guid FamilyId { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Code { get; set; }

    public virtual User? User { get; set; }

    public FamilyResponse ToResponse() =>
        new()
        {
            FamilyId = FamilyId,
            Name = Name,
            Description = Description,
            Code = Code
        };
}