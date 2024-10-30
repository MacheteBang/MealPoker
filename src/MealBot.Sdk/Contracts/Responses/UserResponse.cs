namespace MealBot.Sdk.Contracts.Responses;

public sealed record UserResponse(
    Guid UserId,
    string EmailAddress,
    string AuthProvider,
    string FirstName,
    string LastName);