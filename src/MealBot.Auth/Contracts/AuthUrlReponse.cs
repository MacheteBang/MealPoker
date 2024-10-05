namespace MealBot.Auth.Contracts;

public sealed record AuthUrlReponse(AuthProvider AuthProvider, string Url);