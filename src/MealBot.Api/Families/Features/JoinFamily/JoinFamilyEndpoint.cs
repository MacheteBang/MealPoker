
namespace MealBot.Api.Families.Features.JoinFamily;

internal sealed class JoinFamilyEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch(
            $"{GlobalSettings.RoutePaths.Users}/{{userId:Guid}}{GlobalSettings.RoutePaths.Families}/join/{{familyCode}}",
            async (HttpContext context, Guid userId, string familyCode, ISender sender) =>
            {
                if (!Guid.TryParse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out Guid tokenUserId))
                {
                    return Problem(Identity.Errors.SubMissingFromToken());
                }

                if (userId != tokenUserId)
                {
                    return Problem(Error.Unauthorized());
                }

                var result = await sender.Send(new JoinFamilyCommand(
                    userId,
                    familyCode));

                return result.Match(
                    _ => Results.NoContent(),
                    errors => Problem(errors));
            });
    }
}