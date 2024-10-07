namespace MealBot.Auth.Contracts;

public sealed record TokenResponse(string AccessToken, RefreshTokenResponse RefreshToken);