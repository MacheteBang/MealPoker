namespace MealBot.Auth.Contracts;

public sealed record RefreshTokenResponse(string Value, DateTime ExpiresAt);