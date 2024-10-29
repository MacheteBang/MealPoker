namespace MealBot.Api.Identity.Features.GetUser;

internal sealed class GetUserEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet($"{GlobalSettings.RoutePaths.Users}/{{userId:Guid}}", async (
            HttpContext httpContext,
            ISender sender,
            Guid userId) =>
        {
            if (!Guid.TryParse(httpContext.User.FindFirstValue("nameid"), out var claimUserId))
            {
                return Problem(Errors.NameIdMissingFromToken());
            }

            if (claimUserId != userId)
            {
                return Problem(Errors.UnauthorizedResource("User", userId.ToString()));
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