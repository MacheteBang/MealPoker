namespace MealBot.Api.Meals.Features.GetMeal;

public sealed class GetMealEndpoint : MealBotEndpoint
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(GlobalSettings.RoutePaths.Meals + "/{mealId}", async (Guid mealId, ISender sender) =>
        {
            var result = await sender.Send(new GetMealQuery(mealId));

            return result.Match(
                meal => Results.Ok(meal.ToResponse()),
                errors => Problem(errors));
        });
    }
}