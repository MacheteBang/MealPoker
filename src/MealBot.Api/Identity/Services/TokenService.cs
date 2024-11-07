namespace MealBot.Api.Identity.Services;

internal interface ITokenService
{
    Task<ErrorOr<TokenBundle>> GenerateTokenBundle(User user);
    ErrorOr<Guid> GetUserIdFromAccessToken(string accessToken);
}

internal sealed class TokenService(
    IOptions<AuthorizationOptions> authorizationOptions,
    IUserService userService,
    IHttpContextAccessor httpContextAccessor) : ITokenService
{
    private readonly IOptions<AuthorizationOptions> _authorizationOptions = authorizationOptions;
    private readonly IUserService _userService = userService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<ErrorOr<TokenBundle>> GenerateTokenBundle(User user)
    {
        JwtOptions jwtOptions = _authorizationOptions.Value.JwtOptions;
        RefreshTokenOptions refreshTokenOptions = _authorizationOptions.Value.RefreshTokenOptions;

        string profileImageUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://"
         + $"{_httpContextAccessor.HttpContext?.Request.Host}"
         + $"{GlobalSettings.RoutePaths.Users}/{user.UserId}{GlobalSettings.RoutePaths.ProfileImages}";

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new(JwtRegisteredClaimNames.Email, user.EmailAddress),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new("profile_image", profileImageUrl)
        ];

        var signingKey = Encoding.UTF8.GetBytes(jwtOptions.IssuerSigningKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = jwtOptions.ValidIssuer,
            Audience = jwtOptions.ValidAudience,
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.TokenLifetimeInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature),
        };

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            TokenBundle tokenBundle = new TokenBundle(
                AccessToken: new AccessToken(jwt),
                RefreshToken: new RefreshToken(
                    Value: Guid.NewGuid().ToString(),
                    ExpiresAt: DateTime.UtcNow.AddMinutes(refreshTokenOptions.TokenLifetimeInMinutes)
                )
            );

            user.RefreshToken = tokenBundle.RefreshToken.Value;
            user.RefreshTokenExpiresAt = tokenBundle.RefreshToken.ExpiresAt;
            var updateResult = await _userService.UpdateAsync(user);

            return updateResult.IsError
                ? Errors.JwtCreationFailed("Unable to update user with new refresh token")
                : tokenBundle;
        }
        catch (Exception e)
        {
            return Errors.JwtCreationFailed(e.Message);
        }
    }

    public ErrorOr<Guid> GetUserIdFromAccessToken(string accessToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var token = tokenHandler.ReadJwtToken(accessToken);
            var userIdClaim = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);
            if (userIdClaim is null)
            {
                return Error.Failure("No user ID claim found in token");
            }

            if (!Guid.TryParse(userIdClaim.Value, out Guid userId))
            {
                return Error.Failure("User ID claim is not a valid GUID");
            }

            return userId;
        }
        catch (Exception e)
        {
            return Error.Failure(e.Message);
        }
    }
}