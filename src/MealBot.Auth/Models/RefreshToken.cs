namespace MealBot.Auth.Models;

public sealed record RefreshToken(
    string Value,
    DateTime ExpiresUtc
);