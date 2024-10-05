namespace MealBot.Auth;

public static class Globals
{
    public const string BaseRoute = "/auth";
    public const string SignInLocationRoute = $"{BaseRoute}/signin-locations";

    public const string SignInGoogleRoute = $"{BaseRoute}/sign-in/google";
    public const string RedirectGoogleRoute = $"{BaseRoute}/redirect/google";
    public const string RefreshTokenRoute = $"{BaseRoute}/refresh-token";
    public const string HealthRoute = $"/health{BaseRoute}";
}