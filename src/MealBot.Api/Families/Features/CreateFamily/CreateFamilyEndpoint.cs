namespace MealBot.Api.Families.Features.CreateFamily;

public class CreateFamilyEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(GlobalSettings.RoutePaths.Families, async (HttpContext context, CreateFamilyRequest request, ISender sender) =>
        {
            if (!Guid.TryParse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out Guid userId))
            {
                return Problem(Identity.Errors.SubMissingFromToken());
            }

            var result = await sender.Send(
                new CreateFamilyCommand(
                    userId,
                    request.Name,
                    request.Description)
            );

            return result.Match(
                family => Results.Created($"{GlobalSettings.RoutePaths.Families}/{family.FamilyId}", family.ToResponse()),
                errors => Problem(errors));
        });
    }
}