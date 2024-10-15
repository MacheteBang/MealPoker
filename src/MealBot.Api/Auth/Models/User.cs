namespace MealBot.Api.Auth.Models;

internal sealed class User
{
    private User() { }
    public static User Create(string emailAddress, AuthProvider authProvider, string externalId, string firstName, string lastName, string? pictureUri)
    {
        return new User
        {
            UserId = emailAddress,
            AuthProvider = authProvider,
            ExternalId = externalId,
            EmailAddress = emailAddress,
            FirstName = firstName,
            LastName = lastName,
            PictureUri = pictureUri
        };
    }

    public required string UserId { get; set; }
    public required string EmailAddress { get; set; }
    public AuthProvider AuthProvider { get; set; }
    public required string ExternalId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PictureUri { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }

    public UserResponse ToResponse() => new(
        UserId,
        EmailAddress,
        AuthProvider.ToString(),
        FirstName,
        LastName,
        PictureUri);
}