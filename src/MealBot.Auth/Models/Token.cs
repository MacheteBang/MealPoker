namespace MealBot.Auth.Models;

internal sealed record Token(string AccessToken, RefreshToken RefreshToken);