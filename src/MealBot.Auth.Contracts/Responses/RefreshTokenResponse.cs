namespace MealBot.Auth.Contracts.Responses;

public sealed record RefreshTokenResponse(string Value, DateTime ExpiresAt);