using Microsoft.AspNetCore.Routing;

namespace MealBot.Auth.Features.RefreshAccessToken;

public sealed class RefreshAccessTokenEndpoint : AuthEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(Globals.RefreshTokenRoute, async (HttpContext context) =>
        {
            var refreshAccessTokenRequest = await context.Request.ReadFromJsonAsync<RefreshAccessTokenRequest>();
            var sender = context.RequestServices.GetRequiredService<ISender>();
            var options = context.RequestServices.GetRequiredService<IOptions<RefreshTokenOptions>>();

            if (!context.Request.Cookies.TryGetValue(options.Value.CookieName, out var refreshToken))
            {
                return Problem([Error.Failure()]);
            }

            var query = new RefreshAccessTokenQuery(refreshAccessTokenRequest.AccessToken, refreshToken);

            var result = await sender.Send(query);

            return result.Match(
                token => Results.Ok(token),
                error => Results.Problem());
        });
    }
}