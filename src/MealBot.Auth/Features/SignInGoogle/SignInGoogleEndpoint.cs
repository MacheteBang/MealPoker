using Microsoft.AspNetCore.Routing;

namespace MealBot.Auth.Features.SignInGoogle;

public sealed class SignInGoogleEndpoint : AuthEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(Globals.SignInGoogleRoute, async (HttpContext context) =>
        {
            var authService = context.RequestServices.GetRequiredService<IAuthenticationService>();
            var sender = context.RequestServices.GetRequiredService<ISender>();

            if (!authService.TryGetAuthorizationToken(context.Request.Headers, out string? authorizationToken))
            {
                return Results.Unauthorized();
            }

            var query = new SignInGoogleQuery(authorizationToken!);
            var result = await sender.Send(query);

            return result.Match(
                success => Results.Ok(success.AccessToken),
                error => Results.BadRequest(error));

        });
    }
}