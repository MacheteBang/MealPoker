namespace MealBot.Auth.Options;

internal sealed class AuthenticationOptions
{
    public required GoogleOptions GoogleOptions { get; set; }
}

internal sealed class GoogleOptions
{
    public required string AuthenticationEndpoint { get; set; }
    public required string AuthorizationCodeEndpoint { get; set; }
    public required string ClientId { get; set; }
    public required string ClientSecret { get; set; }
    public required string ResponseType { get; set; }
    public required string Scope { get; set; }
}
