namespace MealBot.Auth.Features.TokenRefresh;

internal sealed class TokenRefreshHandler(ITokenService tokenService, IUserService userService) : IRequestHandler<TokenRefreshQuery, ErrorOr<TokenBundle>>
{
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUserService _userService = userService;

    public async Task<ErrorOr<TokenBundle>> Handle(TokenRefreshQuery query, CancellationToken cancellationToken)
    {
        var userIdResult = _tokenService.GetEmailFromAccessToken(query.OldAccessToken);
        if (userIdResult.IsError)
        {
            return Error.Failure();
        }

        var userResult = await _userService.GetByEmailAddressAsync(userIdResult.Value);
        if (userResult.IsError)
        {
            return Error.Failure();
        }

        var user = userResult.Value;
        if (user.RefreshToken != query.OldRefreshToken || user.RefreshTokenExpiresAt <= DateTime.UtcNow)
        {
            return Error.Failure();
        }

        var accessTokenResult = await _tokenService.GenerateTokenBundle(userResult.Value);
        if (accessTokenResult.IsError)
        {
            return Error.Failure();
        }

        return accessTokenResult.Value;
    }
}