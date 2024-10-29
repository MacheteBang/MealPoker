namespace MealBot.Api.Identity.Models;

internal sealed record RefreshToken(string Value, DateTime ExpiresAt);