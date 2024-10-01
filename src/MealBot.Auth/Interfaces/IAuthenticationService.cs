namespace MealBot.Auth.Interfaces;

public interface IAuthenticationService
{
    Task<ErrorOr<AccessTokenSet>> ExternalSignInAsync(ExternalAuthenticationResponse externalAuthenticationResponse);
    bool TryGetAuthorizationToken(IHeaderDictionary requestHeaders, out string? authorizationToken);
}