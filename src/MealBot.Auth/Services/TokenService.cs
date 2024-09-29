namespace MealBot.Auth.Services;

internal class TokenService(IOptions<AuthorizationOptions> options) : ITokenService
{
    public ErrorOr<string> GenerateAccessToken(User user)
    {
        var config = options.Value.JwtOptions!;

        List<Claim> claims =
        [
            new("id", user.UserId.ToString()),
            new("name", user.EmailAddress),
            new("email", user.EmailAddress)
        ];

        var signingKey = Encoding.UTF8.GetBytes(config.IssuerSigningKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = config.ValidIssuer,
            Audience = config.ValidAudience,
            Expires = DateTime.UtcNow.AddMinutes((double)config.TokenLifetimeInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return jwt;
    }

    public ErrorOr<AccessToken> GenerateRefreshToken(User user)
    {
        var config = options.Value.RefreshTokenOptions!;

        return new AccessToken
        (
            Value: Guid.NewGuid().ToString(),
            ExpiresAt: DateTime.UtcNow.AddMinutes(config.TokenLifetimeInMinutes)
        );
    }

    public ErrorOr<Guid> GetUserId(string accessToken)
    {
        var config = options.Value.JwtOptions!;

        var token = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

        if (token.Issuer != config.ValidIssuer)
        {
            return Errors.InvalidToken();
        }

        var userId = token.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

        return userId is not null
            ? Guid.Parse(userId)
            : Errors.UserNotFoundError();
    }
}