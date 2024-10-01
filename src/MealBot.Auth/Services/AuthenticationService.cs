namespace MealBot.Auth.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    public Task<ErrorOr<AccessTokenSet>> ExternalSignInAsync(ExternalAuthenticationResponse externalAuthenticationResponse)
    {
        throw new NotImplementedException();
    }
}