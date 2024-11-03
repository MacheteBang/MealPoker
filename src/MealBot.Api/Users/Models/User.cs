namespace MealBot.Api.Users.Models;

public sealed class User
{
    private User() { }
    public static User Create(
        string emailAddress,
        AuthProvider authProvider,
        string externalId,
        string firstName,
        string lastName)
    {
        return new User
        {
            UserId = Guid.NewGuid(),
            AuthProvider = authProvider,
            ExternalId = externalId,
            EmailAddress = emailAddress,
            FirstName = firstName,
            LastName = lastName
        };
    }

    public required Guid UserId { get; set; }
    public required string EmailAddress { get; set; }
    public AuthProvider AuthProvider { get; set; }
    public required string ExternalId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public Guid? FamilyId { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }

    public UserResponse ToResponse() => new(
        UserId,
        EmailAddress,
        AuthProvider.ToString(),
        FirstName,
        LastName,
        FamilyId);
}