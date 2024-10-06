namespace MealBot.Auth.Models;

public sealed record Token(string AccessToken, RefreshToken RefreshToken);