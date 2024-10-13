namespace MealBot.Auth.Contracts.Responses;

public sealed record UserResponse(
    string EmailAddress,
    string AuthProvider,
    string FirstName,
    string LastName,
    string? PictureUri);