namespace MealBot.Api.Meals.Features.UnrateMeal;

public sealed class UnrateMealEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete($"{GlobalSettings.RoutePaths.Meals}/{{mealId}}/ratings", async (
            HttpContext context,
            Guid mealId,
            ISender sender) =>
        {
            if (!Guid.TryParse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out Guid userId))
            {
                return Problem(Identity.Errors.SubMissingFromToken());
            }

            var result = await sender.Send(new UnrateMealCommand(
                userId,
                mealId));

            return result.Match(
                _ => Results.Ok(),
                errors => Problem(errors));
        })
        .RequireAuthorization();
    }
}