namespace MealBot.Auth.Models;

public sealed record AccessTokenSet(
    string AccessToken,
    RefreshToken RefreshToken
);