namespace MealBot.Api.Families.Models;

internal class Family
{
    public Guid FamilyId { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Code { get; set; }

    public virtual List<User>? User { get; set; }

    public FamilyResponse ToResponse() =>
        new()
        {
            FamilyId = FamilyId,
            Name = Name,
            Description = Description,
            Code = Code,
            Members = User?.Select(u => new FamilyResponse.FamilyMemberResponse
            {
                UserId = u.UserId,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).ToList()
        };
}