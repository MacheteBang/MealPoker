namespace MealBot.Sdk.Contracts.Responses;

public sealed record RefreshTokenResponse(string Value, DateTime ExpiresAt);