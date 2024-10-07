namespace MealBot.Auth.Contracts;

public sealed record UserResponse(
    string EmailAddress,
    string AuthProvider,
    string FirstName,
    string LastName,
    string? PictureUri);