namespace MealBot.Auth.Features.TokenRefresh;

internal sealed class TokenRefreshEndpoint : AuthEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(Globals.TokenRefreshRoute, async (
            HttpContext context,
            ISender sender,
            IOptions<RefreshTokenOptions> refreshTokenOptions,
            TokenRefreshRequest request) =>
        {

            if (!context.Request.Cookies.TryGetValue(refreshTokenOptions.Value.CookieName, out var refreshToken))
            {
                return Results.Unauthorized();
            }

            var query = new TokenRefreshQuery(request.AccessToken, refreshToken);
            var result = await sender.Send(query);

            if (result.IsError)
            {
                return Results.Unauthorized();
            }

            // TODO: Move this to a common service as it is shared with GetAccessTokenGoogleEndpoint
            var newRefreshToken = result.Value.RefreshToken;
            context.Response.Cookies.Append(refreshTokenOptions.Value.CookieName, newRefreshToken.Value, new()
            {
                Secure = true,
                HttpOnly = true,
                Path = "/auth/tokens/refresh",
                SameSite = SameSiteMode.None,
                Expires = newRefreshToken.ExpiresAt
            });

            AccessTokenResponse accessTokenResponse = new(result.Value.AccessToken.Value);
            return Results.Ok(accessTokenResponse);
        });
    }
}