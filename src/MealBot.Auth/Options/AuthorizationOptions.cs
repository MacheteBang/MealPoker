namespace MealBot.Auth.Options;

internal sealed class AuthorizationOptions
{
    public required JwtOptions JwtOptions { get; set; }
    public RefreshTokenOptions? RefreshTokenOptions { get; set; }
}

internal sealed class JwtOptions
{
    public required string ValidIssuer { get; set; }
    public required string ValidAudience { get; set; }
    public required string IssuerSigningKey { get; set; }
    public required int TokenLifetimeInMinutes { get; set; }
    public required bool ValidateLifetime { get; set; }
    public required bool ValidateIssuer { get; set; }
    public required bool ValidateAudience { get; set; }
    public required bool ValidateSigningKey { get; set; }
}

internal sealed class RefreshTokenOptions
{
    public required string CookieName { get; set; }
    public required int TokenLifetimeInMinutes { get; set; }
}
