namespace MealBot.Auth.Models;

public class User
{
    public Guid UserId { get; init; } = Guid.NewGuid();
    public required string ExternalId { get; set; }
    public required string EmailAddress { get; set; }

    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiresAt { get; set; }
}