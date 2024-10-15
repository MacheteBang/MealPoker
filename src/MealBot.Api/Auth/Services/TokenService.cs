using Errors = MealBot.Api.Auth.DomainErrors.Errors;

namespace MealBot.Api.Auth.Services;

internal interface ITokenService
{
    Task<ErrorOr<TokenBundle>> GenerateTokenBundle(User user);
    ErrorOr<string> GetEmailFromAccessToken(string accessToken);
}

internal sealed class TokenService(
    IOptions<AuthorizationOptions> authorizationOptions,
    IUserService userService) : ITokenService
{
    private readonly IOptions<AuthorizationOptions> _authorizationOptions = authorizationOptions;
    private readonly IUserService _userService = userService;

    public async Task<ErrorOr<TokenBundle>> GenerateTokenBundle(User user)
    {
        JwtOptions jwtOptions = _authorizationOptions.Value.JwtOptions;

        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, user.UserId),
            new(ClaimTypes.Email, user.EmailAddress),
            new(ClaimTypes.GivenName, user.FirstName),
            new(ClaimTypes.Surname, user.LastName),
            new("profilepicture", user.PictureUri ?? string.Empty)
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
                    ExpiresAt: DateTime.UtcNow.AddMinutes(jwtOptions.TokenLifetimeInMinutes)
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

    public ErrorOr<string> GetEmailFromAccessToken(string accessToken)
    {
        throw new NotImplementedException();
    }
}