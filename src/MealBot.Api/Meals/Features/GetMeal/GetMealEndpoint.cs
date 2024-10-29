namespace MealBot.Api.Meals.Features.GetMeal;

public sealed class GetMealEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(GlobalSettings.RoutePaths.Meals + "/{mealId}", async (
            HttpContext context,
            Guid mealId,
            ISender sender) =>
        {
            if (!Guid.TryParse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out Guid userId))
            {
                return Problem(Identity.Errors.SubMissingFromToken());
            }

            var result = await sender.Send(new GetMealQuery(userId, mealId));

            return result.Match(
                meal => Results.Ok(meal.ToResponse()),
                errors => Problem(errors));
        })
        .RequireAuthorization();
    }
}