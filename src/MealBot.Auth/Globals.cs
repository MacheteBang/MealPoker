namespace MealBot.Auth;

public static class Globals
{
    public const string BaseRoute = "/auth";
    public const string AuthUrlRoute = $"{BaseRoute}/urls";
    public const string GetTokenGoogleRoute = $"{BaseRoute}/tokens/google";
    public const string GetUserRoute = $"{BaseRoute}/users";
    public const string TokenRefreshRoute = $"{BaseRoute}/tokens/refresh";

    public const string HealthRoute = $"/health{BaseRoute}";
}