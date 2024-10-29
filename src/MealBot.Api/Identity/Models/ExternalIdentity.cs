namespace MealBot.Api.Identity.Models;

internal record ExternalIdentity(
    AuthProvider AuthProvider,
    string Id,
    string EmailAddress,
    string FirstName,
    string LastName,
    string? ProfilePictureUri);