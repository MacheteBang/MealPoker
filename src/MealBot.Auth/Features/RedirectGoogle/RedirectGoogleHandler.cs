namespace MealBot.Auth.Features.RedirectGoogle;

public sealed class RedirectGoogleHandler(IOptions<AuthenticationOptions> authenticationOptions) : IRequestHandler<RedirectGoogleQuery, ErrorOr<string>>
{
    private readonly IOptions<AuthenticationOptions> _authenticationOptions = authenticationOptions;

    public Task<ErrorOr<string>> Handle(RedirectGoogleQuery request, CancellationToken cancellationToken)
    {
        var options = _authenticationOptions.Value.GoogleOptions!;

        var googleAuthUrl =
            $"{options.AuthenticationEndpoint}?" +
            $"response_type={options.ResponseType}&" +
            $"client_id={options.ClientId}&" +
            $"redirect_uri={options.RedirectUri}&" +
            $"scope={options.Scope}&" +
            $"state={request.State}&" +
            $"nonce={Guid.NewGuid()}";

        return Task.FromResult(googleAuthUrl.ToErrorOr());
    }
}