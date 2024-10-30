namespace MealBot.Api.Families.Features.LeaveFamily;

internal sealed class LeaveFamilyEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            $"{GlobalSettings.RoutePaths.Users}/{{userId:Guid}}{GlobalSettings.RoutePaths.Families}/{{familyId:Guid}}",
            async (HttpContext context, Guid userId, Guid familyId, ISender sender) =>
            {
                if (!Guid.TryParse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out Guid tokenUserId))
                {
                    return Problem(Identity.Errors.SubMissingFromToken());
                }

                if (userId != tokenUserId)
                {
                    return Problem(Error.Unauthorized());
                }

                var command = new LeaveFamilyCommand(userId, familyId);
                var result = await sender.Send(command);

                return result.Match(
                    _ => Results.NoContent(),
                    errors => Problem(errors));
            })
            .RequireAuthorization();
    }
}