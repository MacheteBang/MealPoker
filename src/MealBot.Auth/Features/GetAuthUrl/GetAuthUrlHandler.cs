
namespace MealBot.Auth.Features.GetAuthUrl;

internal sealed class GetAuthUrlHandler(IOptions<AuthenticationOptions> authenticationOptions) : IRequestHandler<GetAuthUrlQuery, ErrorOr<string>>
{
    private readonly IOptions<AuthenticationOptions> _authenticationOptions = authenticationOptions;

    public async Task<ErrorOr<string>> Handle(GetAuthUrlQuery request, CancellationToken cancellationToken)
    {
        // FIXME: Remove async keyword from method signature if no async operations are performed
        await Task.CompletedTask;

        return request.Provider switch
        {
            AuthProvider.Google => HandleGoogle(_authenticationOptions.Value.GoogleOptions!, request.State, request.CallbackUrl),
            _ => Errors.ProviderNotSupported()
        };
    }

    private static string HandleGoogle(GoogleOptions options, string state, string callbackUrl)
    {
        var googleAuthUrl =
            $"{options.AuthenticationEndpoint}?" +
            $"response_type={options.ResponseType}&" +
            $"client_id={options.ClientId}&" +
            $"redirect_uri={callbackUrl}&" +
            $"scope={options.Scope}&" +
            $"state={state}&" +
            $"nonce={Guid.NewGuid()}";

        return googleAuthUrl;
    }
}