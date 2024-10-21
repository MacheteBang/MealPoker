using MealBot.Api.Auth.DomainErrors;

namespace MealBot.Api.Auth.Features.GetToken.Google;

internal sealed class GetAccessTokenEndpoint() : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(GlobalSettings.RoutePaths.TokensGoogle, async (
            HttpContext context,
            ISender sender,
            IOptions<RefreshTokenOptions> refreshTokenOptions,
            string authorizationCode,
            string callBackUri) =>
        {
            if (string.IsNullOrWhiteSpace(authorizationCode))
            {
                return Problem(Errors.GoogleAuthorizationCodeMissing());
            }

            var query = new GetAccessTokenGoogleQuery(authorizationCode, callBackUri);
            var result = await sender.Send(query);

            if (result.IsError)
            {
                return Problem(result.Errors);
            }

            // TODO: Move this to a common service as it is shared with TokenRefreshEndpoint
            var refreshToken = result.Value.RefreshToken;
            context.Response.Cookies.Append(refreshTokenOptions.Value.CookieName, refreshToken.Value, new()
            {
                Secure = true,
                HttpOnly = true,
                Path = "/auth/tokens/refresh",
                SameSite = SameSiteMode.None,
                Expires = refreshToken.ExpiresAt
            });

            AccessTokenResponse accessTokenResponse = new(result.Value.AccessToken.Value);
            return Results.Ok(accessTokenResponse);
        });
    }
}