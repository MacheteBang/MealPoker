namespace MealBot.Auth.Features.GetToken.Google;

internal sealed class GetAccessTokenEndpoint() : AuthEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Globals.GetTokenGoogleRoute, async (
            HttpContext context,
            string authorizationCode,
            string callBackUri,
            ISender sender) =>
        {
            if (string.IsNullOrWhiteSpace(authorizationCode))
            {
                return Results.Unauthorized();
            }

            var query = new GetAccessTokenGoogleQuery(authorizationCode, callBackUri);
            var result = await sender.Send(query);

            if (result.IsError)
            {
                return Results.Unauthorized();
            }

            var refreshTokenOptions = context.RequestServices.GetRequiredService<IOptions<RefreshTokenOptions>>();

            var refreshToken = result.Value.RefreshToken;
            context.Response.Cookies.Append(refreshTokenOptions.Value.CookieName, refreshToken.Value, new()
            {
                Secure = true,
                HttpOnly = true,
                Path = "/api/token/refresh",
                SameSite = SameSiteMode.None,
                Expires = refreshToken.ExpiresAt
            });

            AccessTokenResponse accessTokenResponse = new(result.Value.AccessToken.Value);
            return Results.Ok(accessTokenResponse);
        });
    }
}