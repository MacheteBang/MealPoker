namespace MealBot.Api.Auth.Models;

internal sealed record RefreshToken(string Value, DateTime ExpiresAt);