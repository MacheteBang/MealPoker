namespace MealBot.Auth.Models;

public record ExternalAuthenticationResponse(string ProfilePictureUrl, string ExternalId, string Username, string Email);
