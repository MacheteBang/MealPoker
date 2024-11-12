namespace MealBot.Api.Meals.Features.GetMealRatings;

internal sealed class GetMealRatingsEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(GlobalSettings.RoutePaths.MealRatings, async (ISender sender) =>
        {
            var result = await sender.Send(new GetMealRatingsQuery());

            return result.Match(
                ratings => Results.Ok(ratings),
                errors => Problem(errors));
        })
        .RequireAuthorization();
    }
}