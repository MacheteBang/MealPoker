namespace MealBot.Api.Families.Features.GetFamily;

internal sealed class GetFamilyEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
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

                var query = new GetFamilyQuery(userId, familyId);
                var result = await sender.Send(query);

                return result.Match(
                    family => Results.Ok(family.ToResponse()),
                    errors => Problem(errors));
            });
    }
}