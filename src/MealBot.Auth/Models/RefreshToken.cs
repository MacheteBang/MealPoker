namespace MealBot.Auth.Models;

internal sealed record RefreshToken(string Value, DateTime ExpiresAt);