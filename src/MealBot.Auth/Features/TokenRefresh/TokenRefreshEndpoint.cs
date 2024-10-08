namespace MealBot.Auth.Features.TokenRefresh;

internal sealed class TokenRefreshEndpoint : AuthEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Globals.TokenRefreshRoute, async (
            HttpContext context,
            ISender sender,
            IOptions<RefreshTokenOptions> refreshTokenOptions,
            TokenRefreshRequest request) =>
        {

            if (!context.Request.Cookies.TryGetValue(refreshTokenOptions.Value.CookieName, out var refreshToken))
            {
                return Results.BadRequest();
            }

            var query = new TokenRefreshQuery(request.AccessToken, refreshToken);
            var result = await sender.Send(query);

            return result.Match(
                accessToken => Results.Ok(new AccessTokenResponse(accessToken.Value)),
                errors => Problem(errors));
        });
    }
}