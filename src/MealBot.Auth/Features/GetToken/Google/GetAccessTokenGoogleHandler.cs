namespace MealBot.Auth.Features.GetToken.Google;

internal sealed class GetAccessTokenGoogleHandler(
    IHttpClientFactory httpClientFactory,
    IOptions<AuthenticationOptions> authenticationOptions,
    IAuthenticationService authenticationService,
    ITokenService tokenService) : IRequestHandler<GetAccessTokenGoogleQuery, ErrorOr<TokenBundle>>
{
    private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
    private readonly IOptions<AuthenticationOptions> _authenticationOptions = authenticationOptions;
    private readonly IAuthenticationService _authenticationService = authenticationService;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<ErrorOr<TokenBundle>> Handle(GetAccessTokenGoogleQuery request, CancellationToken cancellationToken)
    {
        GoogleOptions googleOptions = _authenticationOptions.Value.GoogleOptions;

        FormUrlEncodedContent idTokenRequestContent = new
        ([
            new KeyValuePair<string, string>("code", request.AuthorizationCode),
            new KeyValuePair<string, string>("client_id", googleOptions.ClientId),
            new KeyValuePair<string, string>("client_secret", googleOptions.ClientSecret),
            new KeyValuePair<string, string>("redirect_uri", request.CallBackUri),
            new KeyValuePair<string, string>("grant_type", "authorization_code")
        ]);

        // Exchange the Google authorization code for access token
        var authorizationCodeExchangeRequest = await _httpClientFactory.CreateClient().PostAsync(
            googleOptions.AuthorizationCodeEndpoint, idTokenRequestContent, cancellationToken);

        if (!authorizationCodeExchangeRequest.IsSuccessStatusCode)
        {
            var responseMessage = await authorizationCodeExchangeRequest.Content.ReadAsStringAsync(cancellationToken);
            return Errors.GoogleAuthorizationCodeExchangeFailed(responseMessage);
        }

        var googleTokenResponse = await authorizationCodeExchangeRequest.Content.ReadFromJsonAsync<GoogleTokenResponse>(cancellationToken);
        if (googleTokenResponse?.id_token is null)
        {
            return Errors.GoogleIdTokenNotFound();
        }

        GoogleJsonWebSignature.Payload validatedUser;
        try
        {
            validatedUser = await GoogleJsonWebSignature.ValidateAsync
            (
                validationSettings: new() { Audience = [googleOptions.ClientId] },
                jwt: googleTokenResponse?.id_token
            );
        }
        catch (Exception ex)
        {
            return Errors.GoogleIdTokenValidationFailed(ex.Message);
        }

        ExternalIdentity externalIdentity = new ExternalIdentity(
            AuthProvider.Google,
            validatedUser.Subject,
            validatedUser.Email,
            validatedUser.GivenName,
            validatedUser.FamilyName,
            validatedUser.Picture
        );

        var user = await _authenticationService.AddOrGetUserAsync(externalIdentity);
        if (user.IsError)
        {
            return user.Errors;
        }

        return await _tokenService.GenerateTokenBundle(user.Value);
    }
}
