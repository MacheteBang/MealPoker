namespace MealBot.Auth.Features.RefreshAccessToken;

public sealed class RefreshAccessTokenQueryHandler(
    IOptions<RefreshTokenOptions> refreshTokenOptions,
    ITokenService tokenService,
    IUserRepository userRepository) : IRequestHandler<RefreshAccessTokenQuery, ErrorOr<RefreshToken>>
{
    private readonly IOptions<RefreshTokenOptions> _refreshTokenOptions = refreshTokenOptions;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<ErrorOr<RefreshToken>> Handle(RefreshAccessTokenQuery query, CancellationToken cancellationToken)
    {
        var userId = _tokenService.GetUserId(query.AccessToken);

        if (userId.IsError)
        {
            return Errors.InvalidToken();
        }

        var userResult = await _userRepository.GetUserByIdAsync(userId.Value);
        if (userResult.IsError)
        {
            // TODO: Return the correct error here.
            return Errors.InvalidToken();
        }

        var user = userResult.Value;
        if (user.RefreshToken != query.RefreshToken || user.RefreshTokenExpiresAt <= DateTime.UtcNow)
        {
            // TODO: Return the correct error here.
            return Errors.InvalidToken();
        }

        var newAccessTokenResult = _tokenService.GenerateAccessToken(user);
        if (newAccessTokenResult.IsError)
        {
            // TODO: Return the correct error here.
            return Errors.InvalidToken();
        }

        return new RefreshToken(newAccessTokenResult.Value, DateTime.UtcNow.AddHours(1));
    }
}