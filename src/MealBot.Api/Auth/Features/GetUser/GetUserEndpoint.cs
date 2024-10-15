namespace MealBot.Api.Auth.Features.GetUser;

internal sealed class GetUserEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(GlobalSettings.RoutePaths.Users, async (HttpContext httpContext, ISender sender) =>
        {
            string? emailAddress = httpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(emailAddress))
            {
                // TODO: Should this be a different response?
                return Results.Unauthorized();
            }

            var query = new GetUserQuery(emailAddress);
            var result = await sender.Send(query);

            return result.Match(
                user => Results.Ok(user.ToResponse()),
                error => Problem(error));
        })
        .RequireAuthorization();
    }
}