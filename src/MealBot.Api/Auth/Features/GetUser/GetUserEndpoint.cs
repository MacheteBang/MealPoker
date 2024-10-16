namespace MealBot.Api.Auth.Features.GetUser;

internal sealed class GetUserEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{GlobalSettings.RoutePaths.Users}/{{userId:Guid}}", async (
            HttpContext httpContext,
            ISender sender,
            Guid userId) =>
        {
            if (!Guid.TryParse(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out var claimUserId))
            {
                return Results.Unauthorized();
            }

            if (claimUserId != userId)
            {
                return Results.Unauthorized();
            }

            var query = new GetUserQuery(userId);
            var result = await sender.Send(query);

            return result.Match(
                user => Results.Ok(user.ToResponse()),
                error => Problem(error));
        })
        .RequireAuthorization();
    }
}