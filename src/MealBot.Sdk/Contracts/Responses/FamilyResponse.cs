namespace MealBot.Sdk.Contracts.Responses;

public class FamilyResponse
{
    public required Guid FamilyId { get; init; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Code { get; set; }
    public required List<FamilyMemberResponse>? Members { get; set; }

    public class FamilyMemberResponse
    {
        public required Guid UserId { get; init; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}