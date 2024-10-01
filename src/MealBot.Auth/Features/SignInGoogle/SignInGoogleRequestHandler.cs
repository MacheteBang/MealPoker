namespace MealBot.Auth.Features.SignInGoogle;

public sealed class SignInGooglRequestHandler(IHttpClientFactory httpClientFactory, IOptions<AuthenticationOptions> options, IAuthenticationService authenticationService) : IRequestHandler<SignInGoogleQuery, ErrorOr<AccessTokenSet>>
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IOptions<AuthenticationOptions> _options = options;
    private readonly IAuthenticationService _authenticationService = authenticationService;

    public async Task<ErrorOr<AccessTokenSet>> Handle(SignInGoogleQuery query, CancellationToken cancellationToken)
    {
        var externalAuthenticationResponse = await AuthenticateWithGoogleAsync(query.AuthorizationToken, cancellationToken);
        if (externalAuthenticationResponse.IsError)
        {
            return externalAuthenticationResponse.Errors;
        }

        var accessTokenSet = await _authenticationService.ExternalSignInAsync(externalAuthenticationResponse.Value);
        if (accessTokenSet.IsError)
        {
            return accessTokenSet.Errors;
        }

        return accessTokenSet.Value;
    }

    private async Task<ErrorOr<ExternalAuthenticationResponse>> AuthenticateWithGoogleAsync(string authorizationCode, CancellationToken cancellationToken)
    {
        var config = _options.Value.GoogleOptions!;

        var idTokenRequestContent = new FormUrlEncodedContent
        ([
            new KeyValuePair<string, string>("code", authorizationCode),
            new KeyValuePair<string, string>("client_id", config.ClientId),
            new KeyValuePair<string, string>("client_secret", config.ClientSecret),
            new KeyValuePair<string, string>("redirect_uri", config.RedirectUri),
            new KeyValuePair<string, string>("grant_type", "authorization_code")
        ]);

        // Exchange the Google authorization code for access token
        var authorizationCodeExchangeRequest = await _httpClientFactory.CreateClient().PostAsync(
            config.AuthorizationCodeEndpoint, idTokenRequestContent, cancellationToken);

        if (!authorizationCodeExchangeRequest.IsSuccessStatusCode)
        {
            var responseMessage = await authorizationCodeExchangeRequest.Content.ReadAsStringAsync(cancellationToken);
            return Errors.GoogleAuthorizationCodeExchangeFailed(responseMessage);
        }

        var idTokenContent = await authorizationCodeExchangeRequest.Content.ReadFromJsonAsync<GoogleTokenResponse>(cancellationToken);
        if (idTokenContent?.id_token is null)
        {
            return Errors.GoogleIdTokenNotFound();
        }

        try
        {
            var validatedUser = await GoogleJsonWebSignature.ValidateAsync
            (
                validationSettings: new() { Audience = [config.ClientId] },
                jwt: idTokenContent?.id_token
            );

            return new ExternalAuthenticationResponse
            (
                ProfilePictureUrl: validatedUser.Picture,
                ExternalId: validatedUser.Subject,
                Username: validatedUser.Name,
                Email: validatedUser.Email
            );
        }
        catch (InvalidJwtException e)
        {
            return Errors.GoogleIdTokenValidationFailed(e.Message);
        }
    }
}
