
namespace MealBot.Auth.Features.GetSignInLocation;

internal sealed class GetSignInLocationHandler(IOptions<AuthenticationOptions> authenticationOptions) : IRequestHandler<GetSignInLocationQuery, ErrorOr<string>>
{
    private readonly IOptions<AuthenticationOptions> _authenticationOptions = authenticationOptions;

    public async Task<ErrorOr<string>> Handle(GetSignInLocationQuery request, CancellationToken cancellationToken)
    {
        // FIXME: Remove async keyword from method signature if no async operations are performed
        await Task.CompletedTask;

        return request.Provider switch
        {
            Provider.Google => HandleGoogle(_authenticationOptions.Value.GoogleOptions!, request.State, request.ReturnUrl),
            _ => Errors.ProviderNotSupported()
        };
    }

    private static string HandleGoogle(GoogleOptions options, string state, string returnUrl)
    {
        var googleAuthUrl =
            $"{options.AuthenticationEndpoint}?" +
            $"response_type={options.ResponseType}&" +
            $"client_id={options.ClientId}&" +
            $"redirect_uri={returnUrl}&" +
            $"scope={options.Scope}&" +
            $"state={state}&" +
            $"nonce={Guid.NewGuid()}";

        return googleAuthUrl;
    }
}