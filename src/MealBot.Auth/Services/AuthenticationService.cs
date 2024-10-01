namespace MealBot.Auth.Services;

public sealed class AuthenticationService(IOptions<RefreshTokenOptions> refreshTokenOptions) : IAuthenticationService
{
    private readonly IOptions<RefreshTokenOptions> _refreshTokenOptions = refreshTokenOptions;

    public Task<ErrorOr<AccessTokenSet>> ExternalSignInAsync(ExternalAuthenticationResponse externalAuthenticationResponse)
    {
        throw new NotImplementedException();
    }

    public bool TryGetAuthorizationToken(IHeaderDictionary requestHeaders, out string? authorizationToken)
    {
        authorizationToken = null;

        string? authorizationHeaderValue = requestHeaders.Authorization;
        if (authorizationHeaderValue is null)
        {
            return false;
        }

        if (!authorizationHeaderValue.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        authorizationToken = authorizationHeaderValue.Remove(0, "bearer".Length).Trim();
        return true;
    }
}