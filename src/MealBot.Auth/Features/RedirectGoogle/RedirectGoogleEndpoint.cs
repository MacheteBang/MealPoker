using Microsoft.AspNetCore.Routing;

namespace MealBot.Auth.Features.RedirectGoogle;

public sealed class GoogleRedirectEndpoints : AuthEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Globals.RedirectGoogleRoute, async (string state, ISender sender) =>
        {
            var query = new RedirectGoogleQuery(state);
            var result = await sender.Send(query);

            return result.Match(
                url => Results.Ok(url),
                error => Problem(error));
        });
    }
}