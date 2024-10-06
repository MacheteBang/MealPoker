namespace MealBot.Auth.Models;

public record ExternalIdentity(
    AuthProvider AuthProvider,
    string Id,
    string EmailAddress,
    string FirstName,
    string LastName,
    string? ProfilePictureUri);