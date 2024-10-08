namespace MealBot.Auth.Models;

internal sealed class User
{
    public required string EmailAddress { get; set; }
    public AuthProvider AuthProvider { get; set; }
    public required string ExternalId { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? PictureUri { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
}