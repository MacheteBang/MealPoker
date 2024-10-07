namespace MealBot.Auth.Services;

internal interface ITokenService
{
    Task<ErrorOr<AccessToken>> GenerateAccessToken(User user);
}

internal sealed class TokenService(IOptions<AuthorizationOptions> authorizationOptions) : ITokenService
{
    private readonly IOptions<AuthorizationOptions> _authorizationOptions = authorizationOptions;

    public async Task<ErrorOr<AccessToken>> GenerateAccessToken(User user)
    {
        JwtOptions jwtOptions = _authorizationOptions.Value.JwtOptions;

        List<Claim> claims =
        [
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

            return new AccessToken(jwt);
        }
        catch (Exception e)
        {
            return Errors.JwtCreationFailed(e.Message);
        }
    }
}