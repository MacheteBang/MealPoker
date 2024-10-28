namespace MealBot.Api.Meals.Features.DeleteMeal;

public sealed class DeleteMealEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(GlobalSettings.RoutePaths.Meals + "/{mealId}", async (
            HttpContext context,
            Guid mealId,
            ISender sender) =>
        {
            if (!Guid.TryParse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out Guid userId))
            {
                return Problem(Auth.Errors.SubMissingFromToken());
            }

            var result = await sender.Send(new DeleteMealCommand(userId, mealId));

            return result.Match(
                success => Results.NoContent(),
                errors => Problem(errors));
        })
        .RequireAuthorization();
    }
}