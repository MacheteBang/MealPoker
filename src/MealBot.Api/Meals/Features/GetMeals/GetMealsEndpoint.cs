namespace MealBot.Api.Meals.Features.GetMeals;

public sealed class GetMealsEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(GlobalSettings.RoutePaths.Meals, async (
            HttpContext context,
            ISender sender,
            [FromQuery] bool family = false,
            [FromQuery] bool includeCurrentUser = false) =>
        {
            if (!Guid.TryParse(context.User.FindFirstValue(JwtRegisteredClaimNames.Sub), out Guid userId))
            {
                return Problem(Identity.Errors.SubMissingFromToken());
            }

            var result = await sender.Send(new GetMealsQuery(userId, family, includeCurrentUser));

            return result.Match(
                meals => Results.Ok(meals.Select(meal => meal.ToResponse())),
                errors => Problem(errors));
        })
        .RequireAuthorization();
    }
}