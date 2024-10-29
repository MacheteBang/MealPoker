namespace MealBot.Api.Identity.Features.GetAuthUrl;

internal sealed class GetAuthUrlHandler(IOptions<AuthenticationOptions> authenticationOptions) : IRequestHandler<GetAuthUrlQuery, ErrorOr<string>>
{
    private readonly IOptions<AuthenticationOptions> _authenticationOptions = authenticationOptions;

    public Task<ErrorOr<string>> Handle(GetAuthUrlQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult<ErrorOr<string>>(request.Provider switch
        {
            AuthProvider.Google => HandleGoogle(_authenticationOptions.Value.GoogleOptions!, request.State, request.CallbackUri),
            _ => Errors.ProviderNotSupported()
        });
    }

    private static string HandleGoogle(GoogleOptions options, string state, string callbackUri)
    {
        var googleAuthUrl =
            $"{options.AuthenticationEndpoint}?" +
            $"response_type={options.ResponseType}&" +
            $"client_id={options.ClientId}&" +
            $"redirect_uri={Uri.EscapeDataString(callbackUri)}&" +
            $"scope={options.Scope}&" +
            $"state={Uri.EscapeDataString(state)}&" +
            $"nonce={Guid.NewGuid()}";

        return googleAuthUrl;
    }
}