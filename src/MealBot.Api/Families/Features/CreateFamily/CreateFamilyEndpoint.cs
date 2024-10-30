namespace MealBot.Api.Families.Features.CreateFamily;

public class CreateFamilyEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            $"{GlobalSettings.RoutePaths.Users}/{{userId:Guid}}{GlobalSettings.RoutePaths.Families}",
            async (HttpContext context, Guid userId, CreateFamilyRequest request, ISender sender) =>
            {
                if (!Guid.TryParse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out Guid tokenUserId))
                {
                    return Problem(Identity.Errors.SubMissingFromToken());
                }

                if (userId != tokenUserId)
                {
                    return Problem(Error.Unauthorized());
                }

                var result = await sender.Send(
                    new CreateFamilyCommand(
                        userId,
                        request.Name,
                        request.Description)
                );

                return result.Match(
                    family => Results.Created(
                        $"{GlobalSettings.RoutePaths.Users}/{userId}{GlobalSettings.RoutePaths.Families}/{family.FamilyId}",
                        family.ToResponse()),
                    errors => Problem(errors));
            });
    }
}